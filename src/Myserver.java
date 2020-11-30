import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.ArrayList;

public class Myserver {

    public static ArrayList<ServerThread>list =new ArrayList<ServerThread>();
    public  void initServer() {

        try {
            //创建服务器对象,并指定端口号
            ServerSocket server = new ServerSocket(9090);
            System.out.println("服务器已经建立......");
            //不断获取客户端的连接
            while(true){
                Socket socket =server.accept();
                System.out.println("客户端连接进来了......");
                //当有客户端连接进来以后，开启一个线程，用来处理该客户端的逻辑,
                ServerThread st = new ServerThread(socket);
                st.start();
                //添加该客户端到容器中
                list.add(st);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

    }
}

