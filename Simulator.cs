using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;



namespace Rocket_Simulator
{
    class Simulator
    {
        public static double[,] LowerThrust = new double[15, 2];
        public static double[,] UpperThrust = new double[22, 2];
        public static double delay = 4;
        public static string path1 = "C:/Users/ishwa/Downloads/Cesaroni_21062O3400-P.csv";
        public static string path2 = "C:/Users/ishwa/Downloads/Cesaroni_10133M795-P.csv";
        public static string path3 = "C:/Users/ishwa/Downloads/Output.csv";




        public static double tstep = 0.01;




        static void Main(string[] args)
        {
            double time = 0.0;
           

            Rocket first_stage = new Rocket(path1, 6.1, 21041.0, 11.272, 54.326,4.02);
            Rocket second_stage = new Rocket(path2, 12.76, 10133, 4.892, 26.072,2.22);
            Fin lowerstage = new Fin(0.166, 0.3, 0.06, 0.22, 3.72);
            Fin upperstage = new Fin(0.16, 0.3, 0.24, 0.04, 1.9);
            Conditions sim1 = new Conditions();
            System.IO.File.WriteAllText(path3, string.Empty);

            using (StreamWriter writer = new StreamWriter(path3, true))
            {
                writer.WriteLine("time,thrust,drag,coeffcient,A,Az,V,Vz,Altitude");
            }

            Functions.csvReader(path1, LowerThrust);
            Functions.csvReader(path2, UpperThrust);

            sim1.setConditions(first_stage, 5, 0);


            while (time <= first_stage.burn_time + delay)
            {
                    first_stage.avx(time, sim1, 1,time);
                    first_stage.Az = first_stage.A * Math.Cos(first_stage.phi * Math.PI / 180);
                    first_stage.altitude += first_stage.partialD * Math.Cos(first_stage.phi * Math.PI / 180);
                    first_stage.mass = first_stage.totalmass(1);
                    sim1.setConditions(first_stage, 5, 0);
                first_stage.Vz = first_stage.V * Math.Cos(first_stage.phi * Math.PI / 180);

                using (StreamWriter writer = new StreamWriter(path3, true))
                {
                    writer.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8}", time, first_stage.thrust, first_stage.drag(sim1, 1), first_stage.Cd1(sim1), first_stage.A, first_stage.Az, first_stage.V, first_stage.Vz, first_stage.altitude);
                }

                //Console.WriteLine("{0}, {1}", time, first_stage.V);

                time += 0.01;

            }
            second_stage.altitude = first_stage.altitude;
            sim1.setConditions(second_stage, 5, 0);
            second_stage.V = first_stage.V;
            second_stage.Vz = second_stage.V*Math.Cos(second_stage.phi * Math.PI / 180);
         

           
            while (second_stage.Vz>0)
            {
                
                second_stage.avx(time, sim1, 2,first_stage.burn_time+delay);
                second_stage.Az = second_stage.A * Math.Cos(second_stage.phi * Math.PI / 180);
                second_stage.Vz = second_stage.V * Math.Cos(second_stage.phi * Math.PI / 180);
                second_stage.altitude += second_stage.partialD * Math.Cos(second_stage.phi * Math.PI / 180);
                second_stage.mass = second_stage.totalmass(2);
                sim1.setConditions(second_stage, 5, 0);
                time += 0.01;

                using (StreamWriter writer = new StreamWriter(path3, true))
                {
                    writer.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8}", time, second_stage.thrust, second_stage.drag(sim1, 1), second_stage.Cd2(sim1), second_stage.A, second_stage.Az, second_stage.V, second_stage.Vz, second_stage.altitude);
                }




            }

            

            Console.WriteLine("{0:0.00}",second_stage.altitude*3.281);
            Console.WriteLine(time);



        }
    }
    


}


