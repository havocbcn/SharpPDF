using System;
using System.Collections.Generic;

namespace SharpPDF.Lib {
    // 7.3 Objects
    public class Objectizer {
        private readonly Tokenizer tokenizer;

        public Objectizer(Tokenizer tokenizer) {
            this.tokenizer = tokenizer;
        }

        private readonly Dictionary<string, Func<Tokenizer, IPdfObject>> tokenToObject = 
            new Dictionary<string, Func<Tokenizer, IPdfObject>>()
        {
            { "true", (t) => { return new BooleanObject(t); }},
            { "false", (t) => { return new BooleanObject(t); }},
            { "[", (t) => { return new ArrayObject(t); }},
            { "/", (t) => { return new NameObject(t); }},
            { "(", (t) => { return new StringObject(t); }},
            { "<", (t) => {            
                t.SavePosition();           
                t.TokenExcludedCommentsAndWhitespaces();    // se lee el < inicial
                string secondToken = t.TokenExcludedComments().ToString();
                if (secondToken == "<") {
                    t.RestorePosition();
                    return new DictionaryObject(t);
                }
                else {
                    t.RestorePosition();
                    return new StringObject(t);
                }
                }
            }
        };

        public IPdfObject NextObject(bool allowOperators = false)
        {
            tokenizer.SavePosition();            
            Token token = tokenizer.TokenExcludedCommentsAndWhitespaces();

            var validator = new TokenValidator();                

            if (tokenToObject.ContainsKey(token.ToString())) {                
                tokenizer.RestorePosition();
                return tokenToObject[token.ToString()].Invoke(tokenizer);
            }

            if (validator.IsRegularNumber(token)) {
                if (tokenizer.IsEOF())
                {
                    tokenizer.RestorePosition();
                    return new IntegerObject(tokenizer);
                }

                
                Token secondToken = tokenizer.TokenExcludedCommentsAndWhitespaces();
                Token thirdToken = tokenizer.TokenExcludedCommentsAndWhitespaces();

                if (validator.IsRegularNumber(secondToken) && thirdToken.ToString() == "obj")
                {
                    tokenizer.RestorePosition();
                    return new IndirectObject(tokenizer);
                }
                else if (validator.IsRegularNumber(secondToken) && thirdToken.ToString() == "R")
                {
                    tokenizer.RestorePosition();
                    return new IndirectReferenceObject(tokenizer);
                }

                tokenizer.RestorePosition();
                return new IntegerObject(tokenizer);
            }
            else if (validator.IsIntegerNumber(token)) {
                tokenizer.RestorePosition();
                return new IntegerObject(tokenizer);
            }
            else if (validator.IsRealNumber(token)) {
                tokenizer.RestorePosition();
                return new RealObject(tokenizer);
            }
            else if (allowOperators) {
                tokenizer.RestorePosition();
                return new OperatorObject(tokenizer);
            }

            throw new PdfException(PdfExceptionCodes.UNKNOWN_TOKEN, "Not known structure:" + token.ToString());                
        }

        internal bool IsEOF() => tokenizer.IsEOF();
    }
}