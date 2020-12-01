
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.util.ArrayList;
import java.util.concurrent.TimeUnit;

public class ServerThread extends Thread {

    public Socket socket;
    public InputStream ins;
    public OutputStream ous;
    public Database db = new Database();
    public battle_data battle;
    public boolean safe = false;

    public ServerThread(Socket socket) {
        this.socket = socket;
    }

    public void run() {
        try {
            // 获取输入输出流
            ins = socket.getInputStream();
            ous = socket.getOutputStream();
            // 发送要求登录信息给客户端
//            sendMsg(ous,"hi");
            String str = readMsg(ins);
            System.out.println(str);
            String[] array = str.split(",");
            if (!db.userLogin(array[0], array[1], array[2])) {
                str = "wrong account";
                sendMsg(ous, str);
                throw new Exception("wrong login");
            }
            str = readMsg(ins);
            db.op_uid = Integer.parseInt(str);
            str = readMsg(ins);
            if (Myserver.matchList.containsKey(db.uid)) {
                if (Myserver.matchList.get(db.uid) == db.op_uid) {
                    array = str.split(",");
                    ArrayList<int[]> myCharacterList = new ArrayList<>();
                    for (int i = 0; i < array.length / 7; i++) {
                        int[] tmp = new int[7];
                        for (int j = 0; j < 7; j++) {
                            tmp[j] = Integer.parseInt(array[i * 7 + j]);
                        }
                        myCharacterList.add(tmp);
                    }
                    Myserver.playerCharacterList.put(db.uid, myCharacterList);
                    battle = new battle_data(myCharacterList, Myserver.playerCharacterList.get(db.op_uid));
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
                    sendMsg(ous, send.toString());
                    System.out.println(db.uid + " battle end");
                    throw new Exception("end");
                }
            }
            Myserver.matchList.put(db.op_uid, db.uid);
            array = str.split(",");
            ArrayList<int[]> myCharacterList = new ArrayList<>();
            for (int i = 0; i < array.length / 7; i++) {
                int[] tmp = new int[7];
                for (int j = 0; j < 7; j++) {
                    tmp[j] = Integer.parseInt(array[i * 7 + j]);
                }
                myCharacterList.add(tmp);
            }
            Myserver.playerCharacterList.put(db.uid, myCharacterList);
            while (true) {
                for (int i = 0; i < Myserver.list.size(); i++) {
                    System.out.print("");
//                    System.out.println(Myserver.list.get(i).safe);
                    if (!Myserver.list.get(i).safe) continue;
                    if (Myserver.list.get(i).db.op_uid == db.uid && Myserver.list.get(i).battle.GetBattleData() != null) {
                        battle = new battle_data(myCharacterList, Myserver.playerCharacterList.get(db.op_uid));
                        System.out.println(battle);
                        ArrayList<ArrayList<Integer>> battleData = battle.GetBattleData();
                        StringBuilder send = new StringBuilder();
                        for (ArrayList<Integer> battleDatum : battleData) {
                            for (Integer integer : battleDatum) {
                                send.append(integer).append(",");
                            }
                        }
                        safe = true;
                        sendMsg(ous, send.toString());
                        System.out.println(db.uid + " battle end");
                        throw new Exception("end");
                    }
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
            //将当前已经关闭的客户端从容器中移除
            Myserver.list.remove(this);
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
    }

    // 校验客户端输入的账号和密码的函数,由于没有数据库，暂时写死了
    public boolean loginCheck(String name, String pwd) {
        return name.equals("zhou") && pwd.equals("zhou") || name.equals("user") && pwd.equals("pwd")
                || name.equals("huaxinjiaoyu") && pwd.equals("huaxinjiaoyu");
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