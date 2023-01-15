using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaberTest
{
    internal class Program
    {
        class ListNode
        {
            public ListNode Prev;
            public ListNode Next;
            public ListNode Rand; // произвольный элемент внутри списка
            public string Data;
        }


        class ListRand
        {
            public ListNode Head;
            public ListNode Tail;
            public int Count;

            public void Serialize(FileStream s)
            {
                Console.WriteLine("Serialization started");
                Dictionary<ListNode, int> listNodesCounter = new Dictionary<ListNode, int>();
                int counter = 0;
                for (var current = Head; current != null; current = current.Next)
                {
                    listNodesCounter.Add(current, counter);
                    counter++;
                }

                Count = counter + 1;
                BinaryWriter writer = new BinaryWriter(s);
                writer.Write(Count);
                for (var current = Head; current != null; current = current.Next)
                {
                    writer.Write(listNodesCounter[current.Rand]); // Записываем в файл номер узла на который указывает Rand
                    writer.Write(current.Data); // Потом записываем его значение
                }
                Console.WriteLine("Serialization done");
            }

            public void Deserialize(FileStream s)
            {
                Console.WriteLine("Deserialize now");
                Dictionary<int, int> randPos = new Dictionary<int, int>();

                BinaryReader reader = new BinaryReader(s);
                Count = reader.ReadInt32();
                //Console.WriteLine("Count: " + Count);
                ListNode currentNode = new ListNode();
                for (int i = 0; i < Count; i++)
                {
                    int rand = reader.ReadInt32();
                    string data = reader.ReadString();
                    randPos.Add(i, rand);
                    
                    currentNode.Data = data;
                    // Если последний элмент сделать его Tail
                    if (i == Count - 1)
                    {
                        currentNode.Next = null;
                        Tail = currentNode;
                    }
                    else
                    {
                        currentNode.Next = new ListNode();
                    }

                    // Если нулевой элемент, то сделать его Head, а Prev сделать null
                    if (i == 0)
                    {
                        currentNode.Prev = null;
                        Head = currentNode;
                    }
                    else
                    {
                        currentNode.Prev = currentNode;
                        currentNode = currentNode.Next;
                    }
                }

                // Присваивание узлам поле Rand
                currentNode = Head;
                ListNode currentHelperNode = Head;
                int counter = 0;
                // Проходимся по каждому узлу и ищем для каждого необходимый узел Rand, только в худшем случае у нас получится O(n^2)
                for (int i = 0; i < Count; i++)
                {
                    while(counter != randPos[i])
                    {
                        currentHelperNode = currentHelperNode.Next;
                        counter++;
                    }

                    currentNode.Rand = currentHelperNode;
                    //Console.WriteLine("currentNode.Rand after assigment: " + currentNode.Rand);
                    currentHelperNode = Head;
                    counter = 0;
                }
                Console.WriteLine("Deserialization done");
            }
        }


        static void Main(string[] args)
        {
            
        }
    }
}
