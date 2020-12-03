import com.mysql.cj.AppendingBatchVisitor;

import java.util.Random;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.util.ArrayList;

public class ServerThread extends Thread {

    public Socket socket;
    public InputStream ins;
    public OutputStream ous;
    public Database db = new Database();
    public battle_data battle;
    public boolean safe = false;
    public String key;
    public boolean isMy;

    public ServerThread(Socket socket) {
        this.socket = socket;
    }

    public void run() {
        try {
            // 获取输入输出流
            ins = socket.getInputStream();
            ous = socket.getOutputStream();
            // 发送要求登录信息给客户端
            String string;
            String[] array;


            while (true){
                String str = readMsg(ins);
                System.out.println(str);
                switch (str){
                    case "login":
                        string = readMsg(ins);
                        array = string.split(",");
                        if (!loginCheck(array[0], array[1])) {
                            string = "fail";
                            sendMsg(ous, string);
                            break;
                        }
                        string = String.valueOf(db.uid);
                        sendMsg(ous, string);
                        break;
                    case "register":
                        string = readMsg(ins);
                        array = string.split(",");
                        if (!registerCheck(array[3],array[2],array[0],array[1])) {
                            string = "fail";
                            sendMsg(ous, string);
                            break;
                        }
                        string = String.valueOf(db.uid);
                        sendMsg(ous, string);
                        break;
                    case "startBattle":
                        string = readMsg(ins);
                        array = string.split(",");
                        if (Myserver.matchList.containsKey(db.uid)) {
                            ArrayList<int[]> myCharacterList = new ArrayList<>();
                            for (int i = 0; i < array.length / 7; i++) {
                                int[] tmp = new int[7];
                                for (int j = 0; j < 7; j++) {
                                    tmp[j] = Integer.parseInt(array[i * 7 + j]);
                                }
                                myCharacterList.add(tmp);
                            }
                            Myserver.playerCharacterList.put(db.uid, myCharacterList);
                            if (isMy){
                                battle = new battle_data(myCharacterList, Myserver.playerCharacterList.get(db.op_uid));
                            } else {
                                battle = new battle_data(Myserver.playerCharacterList.get(db.op_uid), myCharacterList);
                            }
                            System.out.println(battle);
                            safe = true;
                            ArrayList<ArrayList<Integer>> battleData = battle.GetBattleData();
                            StringBuilder send = new StringBuilder();
                            for (ArrayList<Integer> battleDatum : battleData) {
                                for (Integer integer : battleDatum) {
                                    send.append(integer).append(",");
                                }
                            }
                            while (true){
                                int n = 0;
                                for (int i = 0; i < Myserver.list.size(); i++) {
//                            System.out.println(!Myserver.list.get(i).safe);
                                    if (Myserver.list.get(i).db.op_uid == db.uid && Myserver.list.get(i).safe) {
                                        n = 1;
                                        break;
                                    }
                                }
                                if (n==1) break;
                            }
                            StringBuilder myChess = new StringBuilder();
                            ArrayList<int[]> list = Myserver.playerCharacterList.get(db.op_uid);
                            for (int[] ints : list) {
                                for (int j = 0; j < ints.length; j++) {
                                    myChess.append(ints[j]).append(",");
                                }
                            }
                            sendMsg(ous, send.toString());
                            sendMsg(ous,myChess.toString());
                            System.out.println(db.uid + " battle end");
                            break;
                        }
                        Myserver.matchList.put(db.op_uid, db.uid);
                        array = string.split(",");
                        ArrayList<int[]> myCharacterList = new ArrayList<>();
                        for (int i = 0; i < array.length / 7; i++) {
                            int[] tmp = new int[7];
                            for (int j = 0; j < 7; j++) {
                                tmp[j] = Integer.parseInt(array[i * 7 + j]);
                            }
                            myCharacterList.add(tmp);
                        }
                        Myserver.playerCharacterList.put(db.uid, myCharacterList);
                        boolean loop = true;
                        while (loop) {
                            for (int i = 0; i < Myserver.list.size(); i++) {
                                System.out.print("");
//                    System.out.println(Myserver.list.get(i).safe);
                                if (!Myserver.list.get(i).safe) continue;
                                if (Myserver.list.get(i).db.op_uid == db.uid && Myserver.list.get(i).battle.GetBattleData() != null) {
                                    ArrayList<ArrayList<Integer>> battleData = Myserver.list.get(i).battle.GetBattleData();
                                    StringBuilder send = new StringBuilder();
                                    for (ArrayList<Integer> battleDatum : battleData) {
                                        for (Integer integer : battleDatum) {
                                            send.append(integer).append(",");
                                        }
                                    }
                                    safe = true;
                                    StringBuilder myChess = new StringBuilder();
                                    ArrayList<int[]> list = Myserver.playerCharacterList.get(db.op_uid);
                                    for (int[] ints : list) {
                                        for (int j = 0; j < ints.length; j++) {
                                            myChess.append(ints[j]).append(",");
                                        }
                                    }
                                    sendMsg(ous, send.toString());
                                    sendMsg(ous,myChess.toString());
                                    System.out.println(db.uid + " battle end");
                                    loop = false;
                                    break;
                                }
                            }
                        }
                        if (isMy){
                            Myserver.matchList.remove(db.uid);
                        }
                        break;
                    case "buildRoom":
                        isMy = true;
                        Random random = new Random();
                        key = String.valueOf(random.nextInt(8999)+1000);
                        int [] list = {db.uid,-1};
                        Myserver.roomList.put(key,list);
                        sendMsg(ous,key);
                        new Thread(() -> {
                            try {
                                while (true){
                                    if (!Myserver.roomList.containsKey(key))break;
                                    if (Myserver.roomList.get(key)[1]!=-1){
                                        db.op_uid = Myserver.roomList.get(key)[1];
                                        sendMsg(ous, db.op_uid +","+getUserName(db.op_uid));
                                        break;
                                    }
                                }
                            } catch (Exception e) {
                                e.printStackTrace();
                            }
                        }).start();

                        break;
                    case "quitRoom":
                        Myserver.roomList.remove(key);
                    case "attendRoom":
                        isMy = false;
                        string = readMsg(ins);
                        if (Myserver.roomList.containsKey(string)){
                            int [] l = Myserver.roomList.get(string);
                            l[1] = db.uid;
                            db.op_uid = l[0];
                            sendMsg(ous,db.op_uid +","+getUserName(db.op_uid));
                            Myserver.roomList.put(string,l);
                            key = string;
                        } else {
                            sendMsg(ous,"fail");
                        }
                        break;
                    case "getChess":
                        string = getAllChess();
                        sendMsg(ous, string);
                        break;
                    default:
                }
            }

        } catch (Exception e) {
            System.out.println(db.uid + " Client close\n" + e);
			e.printStackTrace();
        }

        //有异常后统一将流关闭
        try {
            ins.close();
            ous.close();
            socket.close();
            Myserver.roomList.remove(key);
            //将当前已经关闭的客户端从容器中移除
            Myserver.list.remove(this);
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
    }

    private String getUserName(int id){
        return db.getUserName(id);
    }

    private String getAllChess(){
        return db.getAllChess();
    }

    private boolean loginCheck(String name, String pwd) {
        return db.userLogin(name,pwd);
    }

    private boolean registerCheck(String name, String pwd, String email, String phone) {
        if (!db.userRegister(name,pwd,email,phone)){
            return false;
        }
        db.chessAdd(1,50);
        db.chessAdd(2,50);
        db.chessAdd(3,50);
        return true;
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

    //还没有改好，勿用
    public void sendList(OutputStream os, int[] list) throws IOException {
        // 向客户端输出信息

        os.write(13);
        os.write(10);
        os.flush();

    }

    // 读取客户端输入数据的函数
    public String readMsg(InputStream ins) throws Exception {
        // 读取客户端的信息
        int value = ins.read();
        // 读取整行 读取到回车（13）换行（10）时停止读
        StringBuilder str = new StringBuilder();
        while (value != 10) {
            // 点击关闭客户端时会返回-1值
            if (value == -1) {
                throw new Exception();
            }
            str.append((char) value);
            value = ins.read();
        }
        return str.toString().trim();
    }

}