/*
 2019-05-13 
 
 특정 문자열이 출연할 확률 구하기 
 
 

 작성자 서지민


 */




using System;
using System.Collections;
using System.Collections.Generic;

namespace Sequence_Data_Predict
{

    public class Markov
    {

        public List<string> Keys;
        int State_Number;
        int Window_Number = 5;

        int[,] Transition_Count_Table;
        double[,] Transition_Probability_Table;
        double[,] Probability_Sequence_Table;
        List<string> Data_Step;

        public Markov()
        {

        }

        public Markov(List<string> Keys)
        {
            this.Keys = Keys;
            State_Number = Keys.Count;
            Data_Step = new List<string>();
            Probability_Sequence_Table = new double[State_Number, State_Number];

            Transition_Count_Table = new int[State_Number, State_Number];
            Transition_Probability_Table = new double[State_Number, State_Number];
        }

        void Add(string state, string new_state, int count)
        {
            for (int i = 0; i < State_Number; i++)
            {
                for (int j = 0; (Keys[i] == state) && (j < State_Number); j++)
                {
                    if (Keys[j] == new_state)
                    {
                        Transition_Count_Table[i, j] = count;
                        break;
                    }
                }
            }
        }

        void Add(string state, string new_state, double probability)
        {
            for (int i = 0; i < State_Number; i++)
            {
                for (int j = 0; (Keys[i] == state) && (j < State_Number); j++)
                {
                    if (Keys[j] == new_state)
                    {
                        Transition_Probability_Table[i, j] = probability;
                        break;
                    }
                }
            }
        }

        public void Learning()
        {

            //전이누계 구하기
            for (int step = 0; step < Data_Step.Count - 1; step++) //현 상태에서
            {
               
                for (int i = 0; i < State_Number; i++) //해당 상태찾기
                {
                    for (int j = 0; (Keys[i] == Data_Step[step]) && (j < State_Number); j++) //다음 상태 찾기
                    {
                        if (Keys[j] == Data_Step[step + 1])
                        {
                            Transition_Count_Table[i, j] = Transition_Count_Table[i, j] + 1;
                            break;
                        }
                    }
                }
            }

            //전이 확률도 구하기
            double Transition_Count = 0;

            Console.WriteLine("전이누계 \n");
            for (int i = 0; i < State_Number; i++)
            {
                Transition_Count = 0;
                for (int j = 0; j < State_Number; j++)
                {
                    Console.Write(Transition_Count_Table[i, j] + "  ");

                    Transition_Count = Transition_Count + Transition_Count_Table[i, j];
                }
                Console.WriteLine();

                for (int j = 0; j < State_Number; j++)
                {
                    Transition_Probability_Table[i, j] = Transition_Count_Table[i, j] / Transition_Count;
                }

            }
            Console.WriteLine();
            Console.WriteLine();


            Console.WriteLine("전이확률 \n");
            for (int i = 0; i < State_Number; i++)
            {
                for (int j = 0; j < State_Number; j++)
                {
                    Console.Write(Transition_Probability_Table[i, j] + "  ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();


            Console.WriteLine("상태우도 \n");
            for (int i = 0; i < State_Number; i++)
            {
                Console.Write(Keys[i] + " : " + Transition_Likelihood(Keys[i]) + "  ");
                //double sdgs = Transition_Likelihood(Keys[i]);
            }
            Console.WriteLine();
            Console.WriteLine();

        }


        public void Learning(string []data_list)
        {
            
            //전이누계 구하기
            for (int step = 0; step < data_list.Length - 1; step++) //현 상태에서
            {
                Data_Step.Add(data_list[step]);

                for (int i = 0; i < State_Number; i++) //해당 상태찾기
                {
                    for (int j = 0; (Keys[i] == data_list[step]) && (j < State_Number); j++) //다음 상태 찾기
                    {
                        if (Keys[j] == data_list[step + 1])
                        {
                            Transition_Count_Table[i, j] = Transition_Count_Table[i, j] + 1;
                            break;
                        }
                    }
                }
            }

            Data_Step.Add(data_list[data_list.Length - 1]);

            //전이 확률도 구하기
            double Transition_Count = 0;

            Console.WriteLine("전이누계 \n");
            for (int i = 0; i < State_Number; i++)
            {
                Transition_Count = 0;
                for (int j = 0; j < State_Number; j++)
                {
                    Console.Write(Transition_Count_Table[i, j] + "  ");

                    Transition_Count = Transition_Count + Transition_Count_Table[i, j];
                }
                Console.WriteLine();

                for (int j = 0; j < State_Number; j++)
                {
                    Transition_Probability_Table[i, j] = Transition_Count_Table[i, j] / Transition_Count;
                }

            }
            Console.WriteLine();
            Console.WriteLine();


            Console.WriteLine("전이확률 \n");
            for (int i = 0; i < State_Number; i++)
            {
                for (int j = 0; j < State_Number; j++)
                {
                    Console.Write(Transition_Probability_Table[i, j] + "  ");
                    Probability_Sequence_Table[i, j] = Transition_Probability_Table[i, j];
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();


            Console.WriteLine("상태우도 \n");
            for (int i = 0; i < State_Number; i++)
            {
                Console.Write(Keys[i] + " : " +Transition_Likelihood(Keys[i]) + "  ");
                //double sdgs = Transition_Likelihood(Keys[i]);
            }
            Console.WriteLine();
            Console.WriteLine();

        }
        public double[] Probability_Sequence(string state)
        {
            double[] Result = new double[Keys.Count];

            int state_index = 0;
            for (int i = 0; i < Keys.Count; i++)
            {
               if(Keys[i] == state)
                {
                    state_index = i;
                }
            }

            double[,] Result_Table = new double[Keys.Count, Keys.Count];

            //행렬곱셈

            Console.WriteLine("행렬곱셈");
            double sum = 0;
            for(int i =0; i < Keys.Count; i++)
            {
                for (int j = 0; j < Keys.Count; j++)
                {
                    sum = 0;
                    for (int k = 0; k < Keys.Count; k++)
                    {
                        sum = sum + (Probability_Sequence_Table[i, k] * Transition_Probability_Table[k, j]);
                    }

                    Result_Table[i, j] = sum;
                }
            }
     

            for (int i = 0; i < Keys.Count; i++)
            {
                for (int j = 0; j < Keys.Count; j++)
                { 
                    Probability_Sequence_Table[i, j] = Result_Table[i, j];
                    Console.Write(Probability_Sequence_Table[i, j] + "  ");
                }
                Console.WriteLine();
            }

            for (int i = 0; i < Keys.Count; i++)
            {
                Result[i] = Result_Table[state_index, i];
                //Console.Write(Result[i] + "  ");
            }


            Console.WriteLine();
            Console.WriteLine();

            return Result;
        }


        public double Transition_Probability(string state)
        {
            //일단 자기꺼 찾기
            int state_index = 0;
            for(int i = 0; i < Keys.Count; i++)
            {
                if(Keys[i] == state)
                {
                    state_index = i;
                }
               
            }

            //가장 큰 수 찾기
            double MAX = Transition_Probability_Table[state_index, 0];
            for (int i = 0; i < Keys.Count; i++)
            {
                if (MAX < Transition_Probability_Table[state_index, i] )
                {
                    MAX = Transition_Probability_Table[state_index, i];
                }
            }

            return MAX;
        }

        public double Transition_Likelihood(string state)
        {
            //Console.WriteLine("우도 데이터열");
            //Window_Number = Data_Step.Count - 1;
            string[] window = new string[Window_Number];

            for (int i = Data_Step.Count - Window_Number, j = 0; i < Data_Step.Count; i++, j++)
            {
                window[j] = Data_Step[i];
                //Console.Write(window[j] + " -> ");
            }
            //Console.WriteLine();
            //Console.WriteLine();


            double[] Likelihood = new double[Window_Number];
            int state_index = 9999;

            for (int step = 0; step < Window_Number - 1; step++)
            {


                for(int i = 0; i < Keys.Count; i++)
                {
                    if(Keys[i] == window[step])
                    {
                        state_index = i;
                    }
                }

                for (int j = 0; j < Keys.Count; j++)
                {
                    if (Keys[j] == window[step + 1])
                    {
                        Likelihood[step] = Transition_Probability_Table[state_index, j];
                        //Console.Write(Likelihood[step] + "  ->  ");
                    }
                }
            }

            //현재 상태도 추가
            for (int i = 0; i < Keys.Count; i++)
            {
                if (Keys[i] == window[Window_Number - 1])
                {
                    state_index = i;
                    //Console.WriteLine("마지막 문자열: " + Keys[i]);
                }
            }

            for (int i = 0; i < Keys.Count; i++)
            {
                if (Keys[i] == state)
                {
                    Likelihood[Window_Number - 1] = Transition_Probability_Table[state_index, i];
                    //Console.Write("마지막 우도 : " + Likelihood[Window_Number - 1] );
                }
            }

            double state_Likelihood = 1;
            for (int i = 0; i < Window_Number; i++)
            {
                state_Likelihood = state_Likelihood * Likelihood[i];
            }


            return state_Likelihood;
        }

        public double[] Transition_Predict(string state)
        {
            //return Transition_Table[state];
            double[] Result = new double[Keys.Count];

            for(int i = 0; i < Keys.Count; i++)
            {
                Result[i] = Transition_Likelihood(state);
            }

            return Result;
        }

        //학습데이터를 이용
        public double[] Transition_Predict()
        {
            //Learning(Data_Step.ToArray());
            //Learning();
            double[] Result = new double[State_Number];
            double MAX = Transition_Likelihood(Keys[0]);
            string Best = Keys[0];
            for (int i = 0; i < Keys.Count; i++)
            {
                if( MAX <  Transition_Likelihood(Keys[i]))
                {
                    MAX = Transition_Likelihood(Keys[i]);
                    Best = Keys[i];
                }
                Result[i] = Transition_Likelihood(Keys[i]);
            }
            
            

            //double[] Result = Probability_Sequence( Data_Step[  Data_Step.Count - 2]);

            return Result;
        }

        public void Add(string state)
        {
            Data_Step.Add(state);
            //Window_Number++;
        }
    }
}
