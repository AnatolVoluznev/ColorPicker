using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.Res;
using System.IO;

namespace ColorPicker.common
{
    class RgbColor
    {
        private int R;
        private int G;
        private int B;
        private String name;
        private AssetManager assets;
      
        public RgbColor(AssetManager assets)
        {
            this.assets = assets;
            R = 0;
            G = 0;
            B = 0;
            name = "Неизвестный цвет";
        }
        public void setRGB(int R, int G, int B)
        {
            this.R = R;
            this.G = G;
            this.B = B;
        }

        public String getName()
        {
            if (this.name == "Неизвестный цвет")
            {
                this.name = getColorName(R, G, B);
            }
            return name;
        }

        public int getR()
        {
            return R;
        }
        public int getG()
        {
            return G;
        }
        public int getB()
        {
            return B;
        }

        private String getColorName(int R, int G, int B)
        {
            int[] suitableValues = { 0, 0, 0 };
            string line;
            List<String> lines = new List<string>();         
            using (StreamReader reader = new StreamReader(assets.Open("colors.txt")))
            {
                while (true) 
                {
                    line = reader.ReadLine();
                    if (line == null) break;
                    int index = line.IndexOf('#') + 8;
                    int r = getValue(line, index);
                    if (checkValue(R, r))
                    {
                        index += (r.ToString().Length) + 1;
                        int g = getValue(line, index);
                        if (checkValue(G, g))
                        {
                            index += (g.ToString().Length) + 1;
                            int b = getValue(line, index);
                            if (checkValue(B, b))
                            {
                                int[] newValues = { r, g, b };
                                if (isMoreSuitable(suitableValues, newValues))
                                    {
                                    suitableValues = newValues;
                                    index = line.IndexOf('#') - 1;

                                   name = line.Substring(0, index);
                                }
                            }
                        }
                    }

                } 
            }

            return name;
        }
        private bool isMoreSuitable(int[] oldValues, int[] newValues)
        {
            int oldSummary = 0;
            int newSummary = 0;
            for (int i = 0; i < oldValues.Length; i++)
            {
                oldSummary += oldValues[i];
                newSummary += newValues[i];
            }
            if (Math.Abs(oldSummary - R - G - B) > Math.Abs((newSummary - R - G - B)))
                {
                return true;
            }
            else return false;
        

        }


        private int getValue(String line, int index)
        {
            int color = 0;
            Char[] subString = line.Substring(index).ToCharArray();

            int i = 0;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            while (Char.IsDigit(subString[i]))
            {
                sb.Append(subString[i]);
                i++;
                if (i == subString.Length) break;

            }
            try
            {
                color = int.Parse(sb.ToString());
            }
            catch (FormatException e)
            {
                return 0;
            }
            return color;
        }

        private bool checkValue(int valueFromPixel, int valueFromFile)
        {
            if (Math.Abs(valueFromPixel - valueFromFile) < 25)
                return true;
            else return false;

        }

    }
}