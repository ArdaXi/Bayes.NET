/*
 * Copyright (c) 2010 Ariën Holthuizen

 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:

 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace BayesClassifier
{
    public class Classifier
    {
        private readonly Dictionary<string, Category> _categories = new Dictionary<string, Category>();

        private Category GetCategory(string name)
        {
            if (!_categories.ContainsKey(name))
                _categories.Add(name, new Category());
            return _categories[name];
        }
        private int TotalCount()
        {
            return _categories.Sum(kvp => kvp.Value.PhraseCount());
        }
        /// <summary>
        /// Adds a new category, replacing an existing one if necessary.
        /// </summary>
        /// <param name="name">The name of the category.</param>
        /// <param name="phrases">The phrases to add to it.</param>
        public void AddCategory(string name, Dictionary<string, int> phrases)
        {
            if (_categories.ContainsKey(name))
                _categories.Remove(name);
            _categories.Add(name, new Category(phrases));
        }
        /// <summary>
        /// Adds a phrase to a category.
        /// </summary>
        /// <param name="category">The category to add the phrase to. Will be created if necessary.</param>
        /// <param name="phrase">The phrase to add.</param>
        public void Add(string category, string phrase)
        {
            GetCategory(category).Add(phrase);
        }
        /// <summary>
        /// Adds phrases to a category.
        /// </summary>
        /// <param name="category">The category to add the phrases to. Will be created if necessary.</param>
        /// <param name="phrases">The phrases to add.</param>
        public void Add(string category, IEnumerable<string> phrases)
        {
            foreach (string phrase in phrases)
                Add(category, phrase);
        }
        /// <summary>
        /// Classifies the given list, and returns the probability scores.
        /// </summary>
        /// <param name="phrases">The phrases to classify.</param>
        /// <returns>A dictionary with the category names and the probabilities for each, in fractions.</returns>
        public Dictionary<string, double> Classify(IEnumerable<string> phrases)
        {
            var scores = _categories.ToDictionary(kvp => kvp.Key, kvp => 0.0);
            var words = new Category(phrases);
            foreach (KeyValuePair<string, int> word in words)
            {
                foreach (KeyValuePair<string, Category> pair in _categories)
                {
                    Category cat = pair.Value;
                    double count = cat[word.Key];
                    if (count > 0)
                        scores[pair.Key] += Math.Log(count/cat.PhraseCount());
                    else
                        scores[pair.Key] += Math.Log(0.01/cat.PhraseCount());
                }
            }
            foreach (KeyValuePair<string, Category> pair in _categories)
            {
                Category cat = pair.Value;
                if (scores[pair.Key] > 0)
                    scores[pair.Key] += Math.Log((double)cat.PhraseCount()/TotalCount());
            }
            double totalScores = scores.Sum(kvp => kvp.Value);
            return scores.ToDictionary(score => score.Key, score => score.Value/totalScores);
        }
    }
}
