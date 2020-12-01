import java.sql.*;

public class Database {

    private Connection con = null;
    private ResultSet resultSet;

    private String host = "127.0.0.1";
    private String dbname = "los";
    private String user = "root";
    private String pwd = "Xia12345";
    private String port = "3306";

    public int uid;
    public int op_uid;

    private boolean getConnection() {
        try {
            Class.forName("com.mysql.cj.jdbc.Driver");
        } catch (Exception e) {
            System.err.println("Cannot find the MySQL driver. Check CLASSPATH.");
            System.exit(1);
        }

        try {
            String url = "jdbc:mysql://" + host + ":" + port + "/" + dbname+"?useSSL=false&allowPublicKeyRetrieval=true&serverTimezone=Asia/Shanghai";
            con = DriverManager.getConnection(url, user, pwd);
            return true;
        } catch (SQLException e) {
            System.err.println("Database connection failed");
            System.err.println(e.getMessage());
            System.exit(1);
        }
        return false;
    }


    public boolean userLogin(String account, String password, String MAC){
        String sql = "select * from user_data where phone = ? and MAC_address = ? and password = ?";
        if (account.contains("@")){
            sql = "select * from user_data where email = ? and MAC_address = ? and password = ?";
        }
        getConnection();
        try {
            PreparedStatement preparedStatement = con.prepareStatement(sql);
            preparedStatement.setString(1, account);
            preparedStatement.setString(2, MAC);
            preparedStatement.setString(3,password);
            resultSet = preparedStatement.executeQuery();
            ;
            if (resultSet.next()){
                this.uid = resultSet.getInt("uid");
                return true;
            } else {
                System.out.println("Wrong account or password");
                return false;
            }

        } catch (SQLException e) {
            e.printStackTrace();
        } finally {
            closeConnection();
        }
        return true;
    }


    private boolean closeConnection() {
        if (con != null) {
            try {
                con.close();
                con = null;
                return true;
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
        return false;
    }

    public void test(){
        boolean b = true;
        b = b && getConnection();
        b = b && closeConnection();
        if (b)System.out.println("Database initialization successful");
    }
}