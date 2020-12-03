import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.util.Arrays;
import java.util.Scanner;

public class Myclient {
    public static void main(String[] args) {
        Myclient mc = new Myclient();
        mc.initClient();
    }

    public void initClient() {
        try {
            // 创建客户端对象
            Socket client = new Socket("10.20.187.144", 9090);
            // 获取客户端的输入输出流
            final InputStream ins = client.getInputStream();
            final OutputStream ous = client.getOutputStream();
            // 先接收服务器发送的欢迎词
            final Scanner scanner = new Scanner(System.in);
//            System.out.println(readMsg(ins));

//            String str = scanner.nextLine().trim() + "\r\n";
//            sendMsg(ous, str);
//            str = scanner.nextLine().trim() + "\r\n";
//            sendMsg(ous, str);
//            str = scanner.nextLine().trim() + "\r\n";
//            sendMsg(ous, str);
//            String s = readMsg(ins);
//            String[] array = s.split(",");
//            for (int i = 0; i < array.length / 5; i++) {
//                int[] tmp = new int[5];
//                for (int j = 0; j < 5; j++) {
//                    tmp[j] = Integer.parseInt(array[i * 7 + j]);
//                }
//                System.out.println(Arrays.toString(tmp));
//            }

//            // 接收服务端发送过来输入用户名的请求
//            String requestName = readMsg(ins);
//            System.out.println(requestName);
//            // 获取用户名信息，从控制台
//            final Scanner scanner = new Scanner(System.in);
//            String username = scanner.nextLine();
//            // 发送用户名
//            sendMsg(ous, username + "\r\n");
//            // 读取密码请求
//            String requestPwd = readMsg(ins);
//            System.out.println(requestPwd);
//            // 从控制台扫描密码
//            String pwd = scanner.nextLine();
//            // 把密码发送给服务器
//            sendMsg(ous, pwd + "\r\n");
//            // 获取验证结果
//            String result = readMsg(ins);
//            //如果登录失败，则接受服务器端发过来的提示消息
//            while(!result.equals("ok")){
//                //接收"Fail to connect server......"
//                String message=readMsg(ins);
//                System.out.println(message);
//                //接收"please check your name and password and login again....."
//                message=readMsg(ins);
//                System.out.println(message);
//                //接收 "please input your name:""
//                message=readMsg(ins);
//                System.out.println(message);
//                //重新发送用户名给服务器
//                username = scanner.nextLine();
//                // 发送用户名
//                sendMsg(ous, username + "\r\n");
//                //接受密码请求"please input your password:"
//                message=readMsg(ins);
//                System.out.println(message);
//                //发送密码给服务器
//                pwd = scanner.nextLine();
//                // 发送用户名
//                sendMsg(ous, pwd + "\r\n");
//                //接收服务器返的信息
//                result = readMsg(ins);
//            }
//            //如果登录成功，则可以开始聊天了
//            if (result.equals("ok")) {
////				System.out.println("登陆成功");
//                // 发送消息线程
                new Thread(() -> {
                    try {
                        while (true) {
                            // 从控制台扫描一行数据
                            String message = scanner.nextLine();
                            sendMsg(ous, message + "\r\n");
                        }
                    } catch (Exception e) {
                        e.printStackTrace();
                    }

                }).start();
//
                //读取消息线程
                new Thread(() -> {
                    try {
                        while (true) {
                            String message = readMsg(ins);
                            System.out.println(message);
                        }
                    } catch (Exception e) {
                        e.printStackTrace();
                    }
                }).start();
//            } else {
//                System.out.println("登陆失败");
//            }
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    public String readMsg(InputStream ins) throws Exception {
        int value = ins.read();
        String str = "";
        while (value != 10) {
            // 代表客户单不正常关闭
            if (value == -1) {
                throw new Exception();
            }
            str = str + (char) value;
            value = ins.read();
        }
        str = str.trim();
        return str;
    }

    // 发送消息的函数
    public void sendMsg(OutputStream ous, String str) throws Exception {
        byte[] bytes = str.getBytes();
        ous.write(bytes);
        ous.flush();
    }
}
