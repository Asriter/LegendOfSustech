import java.sql.*;

public class Database {

    private Connection con = null;
    private ResultSet resultSet;

    private String host = "10.20.187.144";
    private String dbname = "los";
    private String user = "worker";
    private String pwd = "Los12345";
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

    public String getUserName(int id) {
        getConnection();
        StringBuilder sb = new StringBuilder();
        String sql = "select nick_name from user_data where uid = " + id + ";";
        try {
            Statement statement = con.createStatement();
            resultSet = statement.executeQuery(sql);
            while (resultSet.next()) {
                sb.append(resultSet.getString("nick_name"));
            }
        } catch (SQLException e) {
            e.printStackTrace();
        } finally {
            closeConnection();
        }
        return sb.toString();
    }

    public String getAllChess() {
        getConnection();
        StringBuilder sb = new StringBuilder();
        String sql = "select chess_type,level from chess_data where uid = " + uid + ";";
        try {
            Statement statement = con.createStatement();
            resultSet = statement.executeQuery(sql);
            while (resultSet.next()) {
                sb.append(resultSet.getString("chess_type")).append(',');
                sb.append(resultSet.getString("level")).append(',');
            }
        } catch (SQLException e) {
            e.printStackTrace();
        } finally {
            closeConnection();
        }
        return sb.toString();
    }

    public void chessAdd(int type, int level){
        String sql = "select max(chess_uid) cnt from chess_data;";
        getConnection();
        try {
            PreparedStatement preparedStatement = con.prepareStatement(sql);
            resultSet = preparedStatement.executeQuery();
            resultSet.next();
            int i = resultSet.getInt("cnt") + 1;
            String sql1 = "insert into chess_data values (?,?,?,?)";
            preparedStatement = con.prepareStatement(sql1);
            preparedStatement.setInt(1,i);
            preparedStatement.setInt(2,type);
            preparedStatement.setInt(3,level);
            preparedStatement.setInt(4,uid);
            preparedStatement.executeUpdate();

        } catch (SQLException e) {
            e.printStackTrace();
        } finally {
            closeConnection();
        }
    }

    public boolean userRegister(String name, String pwd, String email, String phone){
        String sql = "select count(*)cnt from user_data where nick_name = ? and email = ? and phone = ? and password = ?;";
        getConnection();
        try {
            PreparedStatement preparedStatement = con.prepareStatement(sql);
            preparedStatement.setString(1, name);
            preparedStatement.setString(2, email);
            preparedStatement.setString(3, phone);
            preparedStatement.setString(4, pwd);
            resultSet = preparedStatement.executeQuery();
            resultSet.next();
            if (resultSet.getInt("cnt")==1){
                return false;
            }
            sql = "select max(uid) cnt from user_data;";
            preparedStatement = con.prepareStatement(sql);
            resultSet = preparedStatement.executeQuery();
            resultSet.next();
            int i = resultSet.getInt("cnt") + 1;
            uid = i;
            String sql1 = "insert into user_data values (?,?,?,?,?)";
            preparedStatement = con.prepareStatement(sql1);
            preparedStatement.setInt(1,i);
            preparedStatement.setString(2, pwd);
            preparedStatement.setString(3, name);
            preparedStatement.setString(4, email);
            preparedStatement.setString(5, phone);
            preparedStatement.executeUpdate();
            return true;

        } catch (SQLException e) {
            e.printStackTrace();
        } finally {
            closeConnection();
        }
        return false;
    }


    public boolean userLogin(String account, String password){
        String sql = "select * from user_data where phone = ? and password = ?";
        if (account.contains("@")){
            sql = "select * from user_data where email = ? and password = ?";
        }
        getConnection();
        try {
            PreparedStatement preparedStatement = con.prepareStatement(sql);
            preparedStatement.setString(1, account);
            preparedStatement.setString(2,password);
            resultSet = preparedStatement.executeQuery();
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
        return false;
    }

//
//    public boolean userLogin(String account, String password, String MAC){
//        String sql = "select * from user_data where phone = ? and MAC_address = ? and password = ?";
//        if (account.contains("@")){
//            sql = "select * from user_data where email = ? and MAC_address = ? and password = ?";
//        }
//        getConnection();
//        try {
//            PreparedStatement preparedStatement = con.prepareStatement(sql);
//            preparedStatement.setString(1, account);
//            preparedStatement.setString(2, MAC);
//            preparedStatement.setString(3,password);
//            resultSet = preparedStatement.executeQuery();
//            if (resultSet.next()){
//                this.uid = resultSet.getInt("uid");
//                return true;
//            } else {
//                System.out.println("Wrong account or password");
//                return false;
//            }
//
//        } catch (SQLException e) {
//            e.printStackTrace();
//        } finally {
//            closeConnection();
//        }
//        return true;
//    }


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