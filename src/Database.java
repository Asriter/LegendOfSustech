import java.sql.*;

public class Database {

    private Connection con = null;
    private ResultSet resultSet;

    private String host = "127.0.0.1";
    private String dbname = "los";
    private String user = "root";
    private String pwd = "Xia12345";
    private String port = "3306";


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