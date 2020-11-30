
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;

public class ServerThread extends Thread {

    public Socket socket;
    public InputStream ins;
    public OutputStream ous;

    public ServerThread(Socket socket) {
        this.socket = socket;
    }

    public void run() {
        try {
            // 获取输入输出流
            ins = socket.getInputStream();
            ous = socket.getOutputStream();
            // 发送消息给客户端
            String msg = "welcome to the server !";
            sendMsg(ous, msg);
            // 发送要求登录信息给客户端
            String userinfo = "please input your name:";
            while (true){
                String str = readMsg(ins);
                System.out.println(str);
            }
//            sendMsg(ous, userinfo);
//            // 获取客户端输入的用户名
//            String userName = readMsg(ins);
//            System.out.println(userName);
//            // 发送要求密码信息给客户端
//            String pwd = "please input your password:";
//            sendMsg(ous,  pwd);
//            // 获取客户端输入的密码
//            String pass = readMsg(ins);
//            // 登录验证
//            boolean falg = loginCheck(userName, pass);
//            // 校验不通过时，循环校验
//            while (!falg) {
//                msg="no";
//                sendMsg(ous, msg);
//                msg = "Fail to connect server......";
//                sendMsg(ous, msg);
//                msg = "please check your name and password and login again.....";
//                sendMsg(ous, msg);
//                msg = "please input your name:";
//                sendMsg(ous, msg);
//                // 获取客户端输入的用户名
//                userName = readMsg(ins);
//                // 发送要求密码信息给客户端
//                msg = "please input your password:";
//                sendMsg(ous, msg);
//                // 获取客户端输入的密码
//                pass = readMsg(ins);
//                falg = loginCheck(userName, pass);
//            }
//
//            //发送登录成功的结果给客户端
//            msg="ok";
//            sendMsg(ous, msg);
//            // 校验成功后：开始聊天
//            msg = "successful connected..... you can chat with your friends now ......";
//            sendMsg(ous, msg);
//            // 聊天处理逻辑
//            //读取客户端发来的消息
//            msg=readMsg(ins);
//            System.out.println("客户端已经接到消息："+msg);
//            //输入bye结束聊天
//            while(!"bye".equals(msg)){
//                //给容器中的每个对象转发消息
//                for (int i = 0; i <Myserver.list.size(); i++) {
//                    ServerThread st =Myserver.list.get(i);
//                    //不该自己转发消息
//                    if(st!=this){
//                        System.out.println("转发消息......");
//                        sendMsg(st.ous, userName+"  is say:"+msg);
//                        System.out.println("转发消息成功......");
//                    }
//                }
//                //等待读取下一次的消息
//                msg=readMsg(ins);
//            }

        } catch (Exception e) {
            System.out.println("客户端不正常关闭......");
//			e.printStackTrace();
        }
        //有异常后统一将流关闭
        try {
            ins.close();
            ous.close();
            socket.close();
            //将当前已经关闭的客户端从容器中移除
            Myserver.list.remove(this);
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
    }

    // 校验客户端输入的账号和密码的函数,由于没有数据库，暂时写死了
    public boolean loginCheck(String name, String pwd) {
        if (name.equals("zhou") && pwd.equals("zhou") || name.equals("user") && pwd.equals("pwd")
                || name.equals("huaxinjiaoyu") && pwd.equals("huaxinjiaoyu")) {

            return true;
        }
        return false;
    }

    // 发送消息的函数
    public void sendMsg(OutputStream os, String s) throws IOException {
        // 向客户端输出信息
        byte[] bytes = s.getBytes();
        os.write(bytes);
        os.write(13);
        os.write(10);
        os.flush();

    }

    // 读取客户端输入数据的函数
    public String readMsg(InputStream ins) throws Exception {
        // 读取客户端的信息
        int value = ins.read();
        // 读取整行 读取到回车（13）换行（10）时停止读
        String str = "";
        while (value != 10) {
            // 点击关闭客户端时会返回-1值
            if (value == -1) {
                throw new Exception();
            }
            str = str + ((char) value);
            value = ins.read();
        }
        str = str.trim();
        return str;
    }

}