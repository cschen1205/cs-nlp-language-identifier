# cs-nlp-language-identifier

A language identifier built using NLP NGram language model

# Install

```bash 
Install-Package cs-nlp-language-identifier
```

# Usage

The sample codes show how to train and use the trained model of language identifier to identifier the language from texts:

```cs 
using System;
using System.Collections.Generic;

namespace LanguageIdentifier
{
    class Program
    {
        static void Main(string[] args)
        {
            NGramLanguageIdentifier lid = new NGramLanguageIdentifier();

            Dictionary<string, List<string>> training_samples = new Dictionary<string, List<string>>();


            training_samples["English"] = new List<string>();
            training_samples["English"].Add("Machine learning is a subfield of computer science (CS) and artificial intelligence (AI) that deals with the construction and study of systems that can learn from data, rather than follow only explicitly programmed instructions. Besides CS and AI, it has strong ties to statistics and optimization, which deliver both methods and theory to the field. Machine learning is employed in a range of computing tasks where designing and programming explicit, rule-based algorithms is infeasible. Example applications include spam filtering, optical character recognition (OCR), search engines and computer vision. Machine learning, data mining, and pattern recognition are sometimes conflated.");

            training_samples["Spanish"] = new List<string>();
            training_samples["Spanish"].Add("El aprendizaje automático es un subcampo de la ciencia de la computación (CS) y la inteligencia artificial (AI) que se ocupa de la construcción y el estudio de los sistemas que se pueden aprender de los datos, en lugar de seguir las instrucciones de manera explícita sólo programados. Además de CS y AI, que tiene fuertes lazos con la estadística y optimización, que ofrecen ambos métodos y la teoría al campo. El aprendizaje automático se emplea en una variedad de tareas de computación, donde el diseño y la programación de algoritmos explícitos, basados ​​en reglas es inviable. Ejemplos de aplicaciones incluyen el filtrado de spam, reconocimiento óptico de caracteres (OCR), los motores de búsqueda y la visión por computador. El aprendizaje automático, minería de datos y reconocimiento de patrones a veces se confunden.");

            training_samples["French"] = new List<string>();
            training_samples["French"].Add("L'apprentissage automatique est un sous-domaine de l'informatique (CS) et de l'intelligence artificielle (IA) qui traite de la construction et de l'étude des systèmes qui peuvent apprendre à partir de données, plutôt que de suivre les instructions que explicitement programmés. Outre CS et AI, il a des liens étroits avec des statistiques et de l'optimisation, qui livrent les deux méthodes et la théorie sur le terrain. L'apprentissage automatique est utilisé dans une gamme de tâches de calcul où la conception et la programmation des algorithmes explicites, basées sur des règles est impossible. Exemples d'applications incluent le filtrage de spam, la reconnaissance optique de caractères (OCR), les moteurs de recherche et la vision par ordinateur. L'apprentissage automatique, l'extraction de données, et la reconnaissance des formes sont parfois confondus.");


            foreach (string language in training_samples.Keys)
            {
                List<string> samples = training_samples[language];
                foreach (string sample in samples)
                {
                    lid.ReadParagraph(sample, language);
                }
            }

            Dictionary<string, List<string>> test_samples = new Dictionary<string, List<string>>();

            test_samples["English"] = new List<string>();
            test_samples["English"].Add("Machine learning tasks can be of several forms. In supervised learning, the computer is presented with example inputs and their desired outputs, given by a teacher, and the goal is to learn a general rule that maps inputs to outputs.");

            test_samples["Spanish"] = new List<string>();
            test_samples["Spanish"].Add("Tareas de aprendizaje de máquinas pueden ser de varias formas. En el aprendizaje supervisado, el equipo se presenta con ejemplos de entradas y sus salidas deseadas, a cargo de un profesor, y el objetivo es aprender una regla general de que los mapas de recursos y resultados.");

            test_samples["French"] = new List<string>();
            test_samples["French"].Add("Tâches d'apprentissage de la machine peuvent être de plusieurs formes. En apprentissage supervisé, l'ordinateur est présenté d'exemples de saisie et des produits souhaités, dispensés par un professeur, et le but est d'apprendre une règle générale qui mappe les entrées aux sorties.");

            foreach (string target_language in test_samples.Keys)
            {
                List<string> samples = test_samples[target_language];
                foreach (string sample in samples)
                {
                    string predicted_language = lid.GetLanguage(sample);
                    Console.WriteLine("predicted: {0}\ttarget: {1}", predicted_language, target_language);
                }
            }
        }
    }
}
```
