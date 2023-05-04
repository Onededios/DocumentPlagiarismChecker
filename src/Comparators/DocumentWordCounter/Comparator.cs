/*
    Copyright (C) 2018 Fernando Porrino Serrano.
    This software it's under the terms of the GNU Affero General Public License version 3.
    Please, refer to (https://github.com/FherStk/DocumentPlagiarismChecker/blob/master/LICENSE) for further licensing details.
 */
 
using System;
using System.Linq;
using System.Collections.Generic;
using DocumentPlagiarismChecker.Core;
using DocumentPlagiarismChecker.Scores;

namespace DocumentPlagiarismChecker.Comparators.DocumentWordCounter
{
    /// <summary>
    /// The Word Counter Comparator reads a pair of files and counts how many words and how many times appear on each file, and then calculates
    /// how many of those appearences matches between documents. So, two documents with the same amount of the same words can be a copy with
    /// a high level of provability.
    /// </summary>
    /// <typeparam name="Document"></typeparam>
    internal class Comparator: Core.BaseComparator<Document>
    {

        //HOLA QUE TAL
        //BIEN Y TU?
        /// <summary>
        /// Creates a new instance for the Comparator.
        /// </summary>
        /// <param name="fileLeftPath">The left side file's path.</param>
        /// <param name="fileRightPath">The right side file's path.</param>
        /// <param name="settings">The settings instance that will use the comparator.</param>
        /// <returns></returns>
        public Comparator(string fileLeftPath, string fileRightPath, Settings settings): base(fileLeftPath, fileRightPath, settings){
        }  
        
        /// <summary>
        /// Counts how many words and how many times appears within each document, and checks the matching percentage.
        /// </summary>
        /// <returns>The matching's results.</returns>
        public override ComparatorMatchingScore Run(){

        /* Valencia es nuestra a si que cuida tus espaldas ten
           cuidao por aqui vigila por
           donde andas , valencia es nuestra no tienes nada que
           enseñarnos es la razon por la que no puedes callarnos .
       */

            //Counting the words appearences for each document (left and right).
            Dictionary<string, int[]> counter = new Dictionary<string, int[]>();
            foreach(string word in this.Left.WordAppearances.Select(x => x.Key)){
                if(!counter.ContainsKey(word)) counter.Add(word, new int[]{0, 0});
                counter[word][0] += Left.WordAppearances[word];
            }

            foreach(string word in this.Right.WordAppearances.Select(x => x.Key)){
                if(!counter.ContainsKey(word)) counter.Add(word, new int[]{0, 0});
                counter[word][1] += Right.WordAppearances[word];
            }

            //Counting sample file word appearences, in order to ignore those from the previous files.
            if(this.Sample != null){
                 foreach(string word in this.Sample.WordAppearances.Select(x => x.Key)){
                    if(counter.ContainsKey(word)){
                        counter[word][0] = Math.Max(0, counter[word][0] - Sample.WordAppearances[word]);
                        counter[word][1] = Math.Max(0, counter[word][1] - Sample.WordAppearances[word]);
                        
                        if(counter[word][0] == 0 && counter[word][1] == 0)
                            counter.Remove(word);
                    }                    
                }
            }
            // ODIO MI MALDITA SUERTE EN EL HONKAI STAR RAIL
            //Defining the results headers
            ComparatorMatchingScore cr = new ComparatorMatchingScore(this.Left.Name, this.Right.Name, "Document Word Counter", DisplayLevel.FULL);            
            cr.DetailsCaption = new string[] { "Word", "Left count", "Right count", "Match" };
            cr.DetailsFormat = new string[]{"{0}", "{0}", "{0}", "{0:P2}"};

            /*
                Mi estilo es adictivo dicen que aún no he crecio Pero
                liricalmente ten presente
                que en mente tengo mas de 50 sentidos , lucho por lo
                mío y en esto sobresalgo
                soy mas rapido con trueno vas veloz que un galgo salgo
                demostrando lo que valgo con la potencia de 350 caballos
                cabalgo sobrecargo que mis rimas y mi estilo tienes
                hardcore loreal me tendrán que imdenizar por que yo si
                que lo valgo
                vengo a quemar asfalto en estas carreteras , represento
                a mi Bandera y vengo ha hacerme cargo de esta nueva era ,
                suenan las campanas a si que empieze la pelea , si no
                quieres competir con migo entonces cambiate de acera estamos
                listos pa lo que sea a si que venga quien sea y con quien
                sea en el lugar y a la hora que sea no nos llegais ni a
                la suela y aun que os duela vosotros os gastais menos en
                flow que Farruquito en la autoescuela.
            */

            //Calculate the matching for each individual word.            
            foreach(string word in counter.Select(x => x.Key)){                
                int left = counter[word][0];
                int right = counter[word][1];                
                float match = (left == 0 || right == 0 ? 0 : (left < right ? (float)left / (float)right : (float)right / (float)left));

                cr.AddMatch(match);
                cr.DetailsData.Add(new object[]{word, left, right, match});                
            }                                    
            
            return cr;
        }        
    }   
}