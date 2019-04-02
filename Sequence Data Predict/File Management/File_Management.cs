/*
2019-03-20

파일 입출력 관련 

1. 학습데이터 파일 읽어오기
2. 예측 데이터 파일 쓰기

작성자 서지민

*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Sequence_Data_Predict
{
    public class File_Management
    {
        FileStream fsR;
        FileStream fsW;

        StreamWriter Writer;
        StreamReader Reader;

        List<string> Read_Data = new List<string>();

        public void Close_File_API()
        {
           
            if (Writer != null)
                Writer.Close();

            if (Reader != null)
                Reader.Close();

            if (fsW != null)
                fsW.Close();

            if (fsR != null)
                fsR.Close();
              
        }

        public void ReadFile(string Fath)
        {
            Close_File_API();

            fsR = new FileStream(Fath, FileMode.Open, FileAccess.Read);
            Reader = new StreamReader(fsR, Encoding.UTF8);
        }

        public void WriteFile(string Fath)
        {
            Close_File_API();

            fsW = new FileStream(Fath, FileMode.Create, FileAccess.Write);
            Writer = new StreamWriter(fsW, Encoding.UTF8);
        }

        public void WriteFile(string Fath, bool append)
        {
            Close_File_API();

            fsW = new FileStream(Fath, FileMode.Append, FileAccess.Write);
            Writer = new StreamWriter(fsW, Encoding.UTF8);
        }

        public List<double[]> Read_NN_Weight()
        {
            string Data = null;
            double[] weight;
            List<double[]> weight_List = new List<double[]>();
            Console.WriteLine("신경망 기억파일 읽기 시작");

            fsR.Seek(0, SeekOrigin.Begin);

            //int ReadFile_Count = 1;
            while (!Reader.EndOfStream)
            {
                Data = Reader.ReadLine();

                if (Data != "" && Data != "\n")
                {
                    Data.Trim();
                    weight = Weight_Split(Data);
                    weight_List.Add(weight);
                }
            }

            Console.WriteLine("신경망 기억파일 읽기 완료 \n\n");

            return weight_List;
        }

        public List<string>  ReadLine(string Fath)
        {
            ReadFile(Fath);

            string Data = "";
            List<string> return_Data = new List<string>();

            fsR.Seek(0, SeekOrigin.Begin);

            while (!Reader.EndOfStream)
            {
                Data = Reader.ReadLine();

                if ((Data != " ") && (Data != " ") && (Data != null))
                {
                    Data.Trim();
                    return_Data.Add(Data);
                }
            }

            return return_Data;
        }


        public void Read_NN_Constructe(out int Width, out int Height, out int Window_Size, out int Stage, out int Kernal_Number, ref int[] Class_Size, out int Result_Length)
        {
            string Data = null;
            Console.WriteLine("신경망 설계파일 읽기 시작");

            fsR.Seek(0, SeekOrigin.Begin);

            while (!Reader.EndOfStream)
            {
                Data = Reader.ReadLine();
                Console.WriteLine(Data);
                Data.Trim();
                Read_Data.Add(Data);
            }

            Console.WriteLine("신경망 파일 읽기 완료 \n\n");

            Width = StringToInt(Read_Data[0]);
            Height = StringToInt(Read_Data[1]);
            Window_Size = StringToInt(Read_Data[2]);
            Stage = StringToInt(Read_Data[3]);
            Kernal_Number = StringToInt(Read_Data[4]);
            Class_Size = Class_Split(Read_Data[5]);
            Result_Length = Class_Size[Class_Size.Length - 1];

            Console.WriteLine("이미지 해상도 " + Width + " * " + Height);
            Console.WriteLine("윈도우 크기 " + Window_Size);
            Console.WriteLine("Convolutional 계층 깊이 " + Stage);
            Console.WriteLine("Full Connected NN 계층 깊이 " + (Class_Size.Length - 1) + "\n");

            Console.WriteLine("각 계층 길이 ");
            for (int i = 0; i < Class_Size.Length; i++)
            {
                if (i == 0)
                {
                    Console.WriteLine("입력계층 " + i + " : " + Class_Size[i]);
                }
                else
                    Console.WriteLine("계층 " + i + " : " + Class_Size[i]);
            }
            Console.WriteLine("\n");

            Console.WriteLine("Full Connected NN 출력길이 " + Result_Length);

            Console.WriteLine("\n\n");
        }


        public List<string[]> Read_NN_Active_Function()
        {
            string Data = null;
            string[] weight;
            List<string[]> weight_List = new List<string[]>();
            Console.WriteLine("신경망 활성함수파일 읽기 시작");

            fsR.Seek(0, SeekOrigin.Begin);

            //int ReadFile_Count = 1;
            while (!Reader.EndOfStream)
            {
                Data = Reader.ReadLine();

                if (Data != "" && Data != "\n")
                {
                    Data.Trim();
                    weight = Active_Function_Split(Data);
                    weight_List.Add(weight);
                }
            }

            Console.WriteLine("신경망 기억파일 읽기 완료 \n\n");

            return weight_List;
        }
        /***************************************************************
                            강화학습
        ***************************************************************/
        public List<double[]> Training_Data_Read(int State_Length, List<int> Data_Action, List<double> Reward)
        {
            string Data = null;

            Console.WriteLine("강화학습자료 파일 읽기 시작");

            List<double[]> State_List = new List<double[]>();

            double[] State;//  = new double[State_Length];
            int Count = 0;

            //Console.WriteLine("총 학습 데이터량 " +  Reward.Length + "\n\n\n");

            fsR.Seek(0, SeekOrigin.Begin);

            while (!Reader.EndOfStream)
            {
                Data = Reader.ReadLine();

                if ((Data != " ") && (Data != " ") && (Data != null))
                {
                    Data.Trim();
                    State = Training_Split(Data, Data_Action, Reward);
                    State_List.Add(State);

                    Count++;
                }
            }

            Console.WriteLine("강화학습자료 파일 읽기 완료 \n\n");

            return State_List;
        }

        /***************************************************************
                            지도학습
        ***************************************************************/
        //지도학습
        public List<double[]> SLTraining_Signal_Read()
        {
            string Data = null;

            Console.WriteLine("지도학습자료 파일 읽기 시작");

            List<double[]> State_List = new List<double[]>();

            double[] State;//  = new double[State_Length];
            int Count = 1;

            //Console.WriteLine("총 학습 데이터량 " +  Reward.Length + "\n\n\n");

            fsR.Seek(0, SeekOrigin.Begin);

            while (!Reader.EndOfStream)
            {
                Data = Reader.ReadLine();

                if ((Data != null) && (Data != " ") && (Data != "\n") && (Data != "end"))
                {
                    Data.Trim();
                    State = SL_Signal_Split(Data);
                    State_List.Add(State);

                    Count++;
                }
            }

            Console.WriteLine("지도학습자료 파일 읽기 완료 \n\n");

            return State_List;
        }

        public int[] Class_Split(string Data)
        {

            Char s = ',';
            string[] split = Data.Split(s);
            int[] classSize = new int[split.Length];
            for (int i = 0; i < split.Length; i++)
            {
                classSize[i] = StringToInt(split[i]);
            }

            return classSize;
        }

        public string[] Active_Function_Split(string Data)
        {

            Char s = ',';
            string[] split = Data.Split(s);
            string[] classSize = new string[split.Length];
            for (int i = 0; i < split.Length; i++)
            {
                classSize[i] = split[i];
            }

            return classSize;
        }

        public double[] Weight_Split(string Data)
        {

            Char s = ' ';
            string[] split = Data.Split(s);
            double[] Weight = new double[split.Length];

            for (int i = 0; i < split.Length; i++)
            {
                try
                {
                    Weight[i] = Convert.ToDouble(split[i].Trim());
                }
                catch (Exception E)
                {
                    Weight[i] = 1.0;
                    Console.WriteLine(E + " Data Write " + 1.0 + "\n\n\n");
                }
            }

            return Weight;
        }

        public double[] Training_Split(string Data, List<int> Data_Action, List<double> Reward)
        {

            Char s = ' ';
            string[] split = Data.Split(s);

            int Length = split.Length;

            double[] State = new double[Length - 2];

            for (int i = 0; i < Length; i++)
            {
                if (i == Length - 2)
                {
                    Data_Action.Add(Int32.Parse(split[i]));
                }
                else if (i == Length - 1)
                {
                    Reward.Add(Double.Parse(split[i]));
                }
                else
                {
                    State[i] = Double.Parse(split[i]);
                }
            }

            return State;

        }

        public double[] SL_Signal_Split(string Data)
        {
            Data.Trim();
            Char s = ' ';
            string[] split = Data.Split(s);

            double[] State = new double[split.Length];

            //Console.WriteLine("데이터 길이=" + split.Length + " 실제 데이터=" + Data);

            for (int i = 0; i < split.Length; i++)
            {
                State[i] = Double.Parse(split[i]);
            }

            return State;

        }
        public void WriteLine(string Data)
        {
            Writer.Flush();
            Writer.WriteLine(Data);

        }
      
        public void Write(string Data)
        {
            Writer.Write(Data);
            //Writer.Flush();
        }

        public void Write(string Data, bool append)
        {
            //fsW.Seek(0, SeekOrigin.End);
            Writer.Write(Data);
            //Writer.Flush();
            //Console.WriteLine("이거 " + Data + " 써지냐?");
        }

        public string ReadLine()
        {
            return Reader.ReadLine();

        }


        public void Weight_Write()
        {

        }

        public int StringToInt(string Data)
        {
            return int.Parse(Data);
        }
    }
}
