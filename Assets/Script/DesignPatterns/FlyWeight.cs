using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//========================================
//##		������ ���� FlyWeight		##
//========================================
/*
    �淮ȭ ���� :
    �Ѱ��� ���� �����͸� �ٸ� ��ü�鿡�� �����ϰ� ����� �޸� ��뷮�� ����
	���� �����͸� �����Ͽ� ����ϱ⿡ �ҷ����� �ð��� ����

    ���� :
    1. ���� ��ü�� ������ ���� �����͸� �����Ͽ� ����
    2. ��ü�� ������ �����͸� ������ �ʰ� ���� �����͸� �����Ͽ� ���
    3. ���� �����͸� �����ϴ� ��ü�� �̱������� ������ ��� ���� ���� ��� ����

    ���� :
    1. ���� ��ü�� ���� �����͸� ������ �ʰ�, ���� �����͸� �����ϱ⿡ �޸𸮸� ������ �� ����
    2. ���� ��ü�� ���� �����͸� �������� �ʰ�, ���� �����͸� �����ϱ⿡ ��ü ������ ������带 ���� �� ����

    ���� :
    1. Ư�� ��ü�� �ٸ� �����͸� ������ ó���ϴ� ���� ����
*/

namespace DesingPattern
{

    public class FlyWeight
    {
        public class MonsterDataContainer
        {
            private static MonsterDataContainer instance;
            public static MonsterDataContainer Instance { get { return instance; } }

            Dictionary<string, MonsterData> dictionary;

            public MonsterDataContainer()   // ���������� ����� Dictionary�� ����
            {
                instance = this;
                dictionary = new Dictionary<string, MonsterData>();

                dictionary.Add("��ũ", new MonsterData("��ũ", 100, 20, 10, "���ں��� �����Դϴ�."));
                dictionary.Add("�巡��", new MonsterData("�巡��", 500, 120, 30, "�극���� ���ؾ� �մϴ�."));
                dictionary.Add("������", new MonsterData("������", 3, 10, 5, "�п��ϴ� �꼺 �����Դϴ�."));
            }

            public MonsterData GetData(string name)
            {
                return dictionary[name];
            }
        }

        public class MonsterData
        {
            public string Name { get; private set; }
            public int MaxHP { get; private set; }
            public int Damage { get; private set; }
            public int Shield { get; private set; }
            public string Desription { get; private set; }

            public MonsterData(string name, int maxHP, int damage, int shield, string desription)
            {
                this.Name = name;
                this.MaxHP = maxHP;
                this.Damage = damage; 
                this.Shield = shield;
                this.Desription = desription;
            }
        }

        public class Monster
        {
            protected MonsterData data;
        }

        public class Orc : Monster
        {
            public Orc()
            {
                this.data = MonsterDataContainer.Instance.GetData("��ũ");        // ���������͸� ���پ�
            }
        }

        public class Dragon : Monster
        {
            public Dragon()
            {
                this.data = MonsterDataContainer.Instance.GetData("�巡��");
            }
        }

        public class Slime : Monster
        {
            public Slime()
            {
                this.data = MonsterDataContainer.Instance.GetData("������");
            }
        }
    }
}