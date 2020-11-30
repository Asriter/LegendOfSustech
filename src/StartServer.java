public class StartServer {
    public static void main(String[] args) {
        Database db = new Database();
        db.test();
        Myserver ms = new Myserver();
        ms.initServer();
    }
}
