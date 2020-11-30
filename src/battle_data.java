import java.util.ArrayList;

public class battle_data
{
    //应该加载的所有角色
    //第一个参数是敌我，0是我1是敌
    //第二个是x坐标，从左到右递增0-2
    //第三个是y坐标，从里到外0-2
    private Character[][][] characterList;

    //处于不知道的什么原因，工厂方法返回的class显示也是null
    //因此额外引入一个list储存该位置有没有单位
    private int[][][] characterListRemember;

    //用于记录在寻找可动角色时是否有找到
    private boolean flag;

    private ArrayList<ArrayList<Integer>> battleData;

    private boolean isWin;

    //用于在得到了数据之后直接创建一个BattleData，不用计算结果
    public battle_data(ArrayList<int[]> myCharacterList, ArrayList<int[]> opponentCharacterList, ArrayList<ArrayList<Integer>> battleData)
    {
        //初始化list
        characterList = new Character[2][3][3];
        characterListRemember = new int[2][3][3];

        //先定义两个list用来装双方的单位，方便遍历
        ArrayList<Character> myList = new ArrayList<Character>();
        ArrayList<Character> opList = new ArrayList<Character>();

        //读取数据并加载到list中
        for (int[] property :  myCharacterList)
        {
            characterList[0][property[1]][property[2]] = CharacterFactory.CreateCharacter(property[0]);
            //暂时不写属性操控方法

            //在remember中记录
            characterListRemember[0][property[1]][property[2]] = 1;
            //加入该方法的list
            myList.add(characterList[0][property[1]][property[2]]);
          //在单位中加入所在的location
            int[] location = {0, property[1], property[2]};
            characterList[0][property[1]][property[2]].SetLocation(location);

        }
        for(int[] property : opponentCharacterList)
        {
            characterList[1][property[1]][property[2]] = CharacterFactory.CreateCharacter(property[0]);
            //暂时不写属性操控方法

            //在remember中记录
            characterListRemember[1][property[1]][property[2]] = 1;
            //加入该方法的list
            opList.add(characterList[1][property[1]][property[2]]);
          //在单位中加入所在的location
            int[] location = {1, property[1], property[2]};
            characterList[1][property[1]][property[2]].SetLocation(location);
        }

        //测试用
        //this.battleData = GenerateBattleData();
        this.battleData = battleData;
    }

    //传来的参数为两个List，每个List中都是长度为7的int数组
    //id, x, y, hp, atk, def, cri
    public battle_data(ArrayList<int[]> myCharacterList, ArrayList<int[]> opponentCharacterList)
    {
    	//初始化list
        characterList = new Character[2][3][3];
        characterListRemember = new int[2][3][3];

        //先定义两个list用来装双方的单位，方便遍历
        ArrayList<Character> myList = new ArrayList<Character>();
        ArrayList<Character> opList = new ArrayList<Character>();

        //读取数据并加载到list中
        for (int[] property :  myCharacterList)
        {
            characterList[0][property[1]][property[2]] = CharacterFactory.CreateCharacter(property[0]);
            //暂时不写属性操控方法

            //在remember中记录
            characterListRemember[0][property[1]][property[2]] = 1;
            //加入该方法的list
            myList.add(characterList[0][property[1]][property[2]]);
          //在单位中加入所在的location
            int[] location = {0, property[1], property[2]};
            characterList[0][property[1]][property[2]].SetLocation(location);

        }
        for(int[] property : opponentCharacterList)
        {
            characterList[1][property[1]][property[2]] = CharacterFactory.CreateCharacter(property[0]);
            //暂时不写属性操控方法

            //在remember中记录
            characterListRemember[1][property[1]][property[2]] = 1;
            //加入该方法的list
            opList.add(characterList[1][property[1]][property[2]]);
          //在单位中加入所在的location
            int[] location = {1, property[1], property[2]};
            characterList[1][property[1]][property[2]].SetLocation(location);
        }
        
        //测试用
        //this.battleData = GenerateBattleData();
        this.battleData = GenerateBattleData(myList, opList);
    }

    //尝试另一种实现方式，生成battleData
    private ArrayList<ArrayList<Integer>> GenerateBattleData(ArrayList<Character> myList, ArrayList<Character> opList)
    {
        battleData = new ArrayList<ArrayList<Integer>>();

        //根据速度得到行动顺序，0是动，1是对方动
        //每次得到data之后都会取反
        int isAttack = GetSpeed();

        int[] myIndex = { -1 };
        int[] opIndex = { -1 };

        //模拟战斗，得到battledata
        while (true)
        {
            Character atkCharacter = null;
            //我方动
            if (isAttack == 0)
            {
                atkCharacter = FindActionCharacter(myList, myIndex);
                isAttack = 1;
            }
            //对方动
            else
            {
                atkCharacter = FindActionCharacter(opList, opIndex);
                isAttack = 0;
            }

            if (!flag)
            {
                //Debug.Log("ListLength: " + battleData.Count);
                return battleData;
            }

            //没打完
            ArrayList<Integer> list = atkCharacter.Action(this);
            //Debug.Log(list);
            battleData.add(list);
        }
    }


    //尝试另一种实现方式
    private Character FindActionCharacter(ArrayList<Character> list, int[] index)
    {
        flag = false;

        for (int i = index[0] + 1; i < list.size(); i++)
        {
            if (list.get(i).GetHp() <= 0)
                continue;

            flag = true;
            index[0] = i;
            return list.get(i);
        }

        for (int i = 0; i <= index[0]; i++)
        {
            if (list.get(i).GetHp() <= 0)
                continue;

            flag = true;
            index[0] = i;
            return list.get(i);
        }
        return null;
    }

    //获取双方速度来决定谁先动，return 0我先动，1对方先动
    private int GetSpeed()
    {
        int count1 = 0;
        int count2 = 0;
        double mySpeed = 0;
        double opSpeed = 0;
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (this.hasCharacterInGrid(0, x, y))
                {
                    count1 += 1;
                    mySpeed += characterList[0][x][y]._speed;
                }
                if (this.hasCharacterInGrid(1, x, y))
                {
                    count2 += 1;
                    opSpeed += characterList[1][x][y]._speed;
                }
            }
        }
        mySpeed = mySpeed / count1;
        opSpeed = opSpeed / count2;

        return mySpeed >= opSpeed ? 0 : 1;
    }

    public Character[][][] GetCharacterList()
    {
        return characterList;
    }

    public ArrayList<ArrayList<Integer>> GetBattleData()
    {
        return this.battleData;
    }

    public boolean hasCharacterInGrid(int x, int y, int z)
    {
        return characterListRemember[x][y][z] == 1;
    }
}
