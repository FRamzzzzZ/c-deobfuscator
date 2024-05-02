using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace de_obf
{
    internal class Deobf
    {
        private string filePath;

        public Deobf(string filePath)
        {
            this.filePath = filePath;
        }

        public void StartDeobfuscation()
        {
            Console.WriteLine("Starting deobfuscation process...");
            AnalyzeAndDeobfuscate();
        }

        private void AnalyzeAndDeobfuscate()
        {
            try
            {
                string obfuscatedCode = File.ReadAllText(filePath);

                // Translate non-English identifiers to English
                obfuscatedCode = TranslateToEnglish(obfuscatedCode);

                // Perform deobfuscation steps
                obfuscatedCode = DecryptStringEncryption(obfuscatedCode);
                obfuscatedCode = RenameIdentifiers(obfuscatedCode);
                obfuscatedCode = ReconstructControlFlow(obfuscatedCode);
                obfuscatedCode = DynamicAnalysis(obfuscatedCode);
                obfuscatedCode = RemoveUnusedVariables(obfuscatedCode);
                obfuscatedCode = RemoveUnusedMethods(obfuscatedCode);
                obfuscatedCode = SimplifyExpressionsAndOperations(obfuscatedCode);
                obfuscatedCode = InlineFunctions(obfuscatedCode);
                obfuscatedCode = ReplaceObfuscatedConstants(obfuscatedCode);
                obfuscatedCode = RemoveDebuggingCode(obfuscatedCode);
                obfuscatedCode = DeobfuscateNamingPatterns(obfuscatedCode);
                obfuscatedCode = SimplifyControlFlow(obfuscatedCode);
                obfuscatedCode = OptimizeLoops(obfuscatedCode);
                obfuscatedCode = RemoveUnusedImports(obfuscatedCode);
                obfuscatedCode = RestoreObfuscatedCodeStructure(obfuscatedCode);

                // Save deobfuscated code to a text file
                string deobfuscatedFilePath = Path.ChangeExtension(filePath, ".deobfuscated.txt");
                File.WriteAllText(deobfuscatedFilePath, obfuscatedCode);

                // Display the deobfuscation process status
                Console.WriteLine($"Deobfuscation process {Path.GetFileName(filePath)} complete.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during deobfuscation: {ex.Message}");
            }
        }

        private string DecryptStringEncryption(string code)
        {
            Regex regex = new Regex(@"Encrypt\(""(.*?)""\)");
            return regex.Replace(code, match =>
            {
                string encryptedString = match.Groups[1].Value;
                string decryptedString = DecryptXOREncryption(encryptedString);
                return "\"" + decryptedString + "\"";
            });
        }

        private string RenameIdentifiers(string code)
        {
            // Define patterns for identifiers to be replaced and their replacements
            Dictionary<string, string> replacements = new Dictionary<string, string>
            {
                { @"\bvarName\b", "variableName" },     // Replace whole word "varName" with "variableName"
                { @"\bvarCount\b", "countOfVariables" }, // Replace whole word "varCount" with "countOfVariables"
                { @"\bmethodName\b", "functionName" },   // Replace whole word "methodName" with "functionName"
                // Add more patterns and replacements as needed
            };

            // Iterate over each pattern and replacement
            foreach (var replacement in replacements)
            {
                // Replace all occurrences of the identifier using the pattern
                code = Regex.Replace(code, replacement.Key, replacement.Value, RegexOptions.IgnoreCase);
            }

            // Return the code with identifiers renamed
            return code;
        }

        private string ReconstructControlFlow(string code)
        {
            // Placeholder for reconstructing control flow
            // This method is supposed to reconstruct the control flow of the provided code
            // For now, it removes redundant branches in if-else statements and simplifies loops

            // Define regular expressions to match if-else statements and loops
            Regex ifElseRegex = new Regex(@"if\s*\(.*?\)\s*{(?:[^{}]+|(?<Open>{)|(?<-Open>}))*(?(Open)(?!))\s*(?:else\s*{(?:[^{}]+|(?<Open>{)|(?<-Open>}))*(?(Open)(?!))|(?<!(?:if|else)).*)");
            Regex loopRegex = new Regex(@"(?:for|while)\s*\(.*?\)\s*{(?:[^{}]+|(?<Open>{)|(?<-Open>}))*(?(Open)(?!))");

            // Replace redundant branches in if-else statements with the appropriate branch
            string reconstructedCode = ifElseRegex.Replace(code, match =>
            {
                string ifBlock = match.Value;

                // Check if the else block is empty or contains only whitespace
                if (Regex.IsMatch(ifBlock, @"else\s*{\s*}"))
                {
                    // Remove the else block if it's empty or contains only whitespace
                    return ifBlock.Replace("else", "");
                }
                else
                {
                    // If the else block is not empty or whitespace, keep the if-else statement unchanged
                    return ifBlock;
                }
            });

            // Remove redundant or unnecessary constructs in loops
            reconstructedCode = loopRegex.Replace(reconstructedCode, match =>
            {
                string loopBlock = match.Value;

                // Check if the loop block is empty or contains only whitespace
                if (Regex.IsMatch(loopBlock, @"\(\s*\)\s*{\s*}"))
                {
                    // Remove the loop construct if it's empty or contains only whitespace
                    return "";
                }
                else
                {
                    // If the loop block is not empty or whitespace, keep the loop construct unchanged
                    return loopBlock;
                }
            });

            // Return the reconstructed code
            return code;
        }

        private string DynamicAnalysis(string code)
        {
            // Perform dynamic analysis to identify and extract potential hardcoded values

            // Extract string literals
            var stringLiterals = Regex.Matches(code, @"""([^""\\]|\\.)*""", RegexOptions.Multiline)
                                    .Cast<Match>()
                                    .Select(m => m.Value)
                                    .Distinct()
                                    .ToList();

            // Extract numeric constants
            var numericConstants = Regex.Matches(code, @"\b\d+(\.\d+)?\b")
                                     .Cast<Match>()
                                     .Select(m => m.Value)
                                     .Distinct()
                                     .ToList();

            // Print the identified hardcoded values
            Console.WriteLine("Identified hardcoded values:");
            Console.WriteLine("String literals:");
            foreach (var literal in stringLiterals)
            {
                Console.WriteLine(literal);
            }
            Console.WriteLine("Numeric constants:");
            foreach (var constant in numericConstants)
            {
                Console.WriteLine(constant);
            }

            // You can further process or analyze the extracted values as needed

            return code;
        }

        private string RemoveUnusedVariables(string code)
        {
            code = Regex.Replace(code, @"\b(?:int|double|string|bool|var)\s+(\w+)\s*;", match =>
            {
                string variableName = match.Groups[1].Value;
                if (!Regex.IsMatch(code, @"\b" + variableName + @"\b"))
                {
                    // Variable is not used, so remove its declaration
                    return "";
                }
                else
                {
                    // Variable is used, so keep its declaration
                    return match.Value;
                }
            });

            return code;
        }

        private string RemoveUnusedMethods(string code)
        {
            Dictionary<string, bool> methodCalled = new Dictionary<string, bool>();

            code = Regex.Replace(code, @"\b(\w+)\s*\(\s*\)", match =>
            {
                string methodName = match.Groups[1].Value;
                if (!methodCalled.ContainsKey(methodName))
                {
                    methodCalled[methodName] = true;
                }
                return match.Value;
            });

            // Remove method definitions that are not called
            code = Regex.Replace(code, @"void\s+(\w+)\s*\(\s*\)\s*{([^}]*)}", match =>
            {
                string methodName = match.Groups[1].Value;
                if (!methodCalled.ContainsKey(methodName))
                {
                    // Method is not called, so remove its definition
                    return "";
                }
                else
                {
                    // Method is called, so keep its definition
                    return match.Value;
                }
            }, RegexOptions.Singleline);

            return code;
        }

        private string SimplifyExpressionsAndOperations(string code)
        {
            Regex arithmeticRegex = new Regex(@"(\d+)\s*([+\-*\/])\s*(\d+)");
            code = arithmeticRegex.Replace(code, match =>
            {
                int operand1 = int.Parse(match.Groups[1].Value);
                int operand2 = int.Parse(match.Groups[3].Value);
                char operation = match.Groups[2].Value[0];
                int result = 0;

                switch (operation)
                {
                    case '+':
                        result = operand1 + operand2;
                        break;
                    case '-':
                        result = operand1 - operand2;
                        break;
                    case '*':
                        result = operand1 * operand2;
                        break;
                    case '/':
                        if (operand2 != 0)
                            result = operand1 / operand2;
                        else
                            Console.WriteLine("Division by zero encountered!");
                        break;
                }

                return result.ToString();
            });

            return code;
        }

        private string InlineFunctions(string code)
        {
            Dictionary<string, string> functionDefinitions = new Dictionary<string, string>();
            Regex functionDefinitionRegex = new Regex(@"void\s+(\w+)\s*\(\)\s*{([^}]*)}");
            foreach (Match match in functionDefinitionRegex.Matches(code))
            {
                string functionName = match.Groups[1].Value;
                string functionBody = match.Groups[2].Value;
                functionDefinitions[functionName] = functionBody;
            }

            Regex functionCallRegex = new Regex(@"(\w+)\s*\(\)");
            code = functionCallRegex.Replace(code, match =>
            {
                string functionName = match.Groups[1].Value;
                if (functionDefinitions.ContainsKey(functionName))
                {
                    return functionDefinitions[functionName];
                }
                else
                {
                    return match.Value;
                }
            });

            return code;
        }

        private string DecryptXOREncryption(string encryptedString)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(encryptedString);
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] ^= 0x5A;
            }
            return Encoding.UTF8.GetString(bytes);
        }

        private string TranslateToEnglish(string code)
        {
            Dictionary<string, string> translationDictionary = new Dictionary<string, string>
            {
                // Add translations for non-English identifiers
                { "varName", "variableName" },
                { "methodName", "functionName" },
                // Add more translations as needed
            };

            foreach (var translation in translationDictionary)
            {
                code = code.Replace(translation.Key, translation.Value);
            }

            return code;
        }

        private string ReplaceObfuscatedConstants(string code)
        {
            // Replace obfuscated constants with meaningful values

            // Replace common obfuscated constants with meaningful values
            code = code.Replace("123456", "42"); // Example: Replace an obfuscated integer constant with a common meaningful value
            code = code.Replace("abcdef", "3.14159"); // Example: Replace an obfuscated constant with the value of Pi
            code = code.Replace("xyz", "\"Hello, World!\""); // Example: Replace an obfuscated string constant with a greeting message

            // Replace commonly obfuscated numeric constants
            code = Regex.Replace(code, @"\b[0-9a-fA-F]{8}\b", "0xABCDEF"); // Example: Replace a hexadecimal constant with a placeholder value

            // Replace commonly obfuscated string constants
            code = code.Replace("super_secret_password", "\"my_password\""); // Example: Replace an obfuscated password constant with a more descriptive value
            code = code.Replace("s3cr3t_t0k3n", "\"my_token\""); // Example: Replace an obfuscated token constant with a more descriptive value

            // Add more replacements for other obfuscated constants here

            return code;
        }

        private string RemoveDebuggingCode(string code)
        {
            // Remove debugging-related code statements from the code

            // Remove Console.WriteLine and Console.Write statements
            code = Regex.Replace(code, @"Console\.Write(Line)?\((.*?)\);", "");

            // Remove Debug.WriteLine and Debug.Write statements
            code = Regex.Replace(code, @"Debug\.Write(Line)?\((.*?)\);", "");

            // Remove System.Diagnostics.Trace.WriteLine and System.Diagnostics.Trace.Write statements
            code = Regex.Replace(code, @"System\.Diagnostics\.Trace\.Write(Line)?\((.*?)\);", "");

            return code;
        }

        private string DeobfuscateNamingPatterns(string code)
        {
            // Deobfuscate specific naming patterns used in the code

            // Replace obfuscated variable names with meaningful names
            code = Regex.Replace(code, @"\b(?:[a-z]{3}[0-9]{2})\b", match =>
            {
                string obfuscatedName = match.Value;
                string deobfuscatedName = DeobfuscateVariableName(obfuscatedName);
                return deobfuscatedName;
            });

            // Replace other common obfuscated patterns with meaningful names
            code = Regex.Replace(code, @"\b(?:_[a-z]{3}[0-9]{2}_)\b", "importantVariable");
            code = Regex.Replace(code, @"\b(?:[A-Z]{2}[0-9]{3})\b", "configurationValue");
            code = Regex.Replace(code, @"\b(?:[0-9]{2}_\w{4})\b", "userData");

            return code;
        }

        private string DeobfuscateVariableName(string obfuscatedName)
        {
            // Deobfuscate variable names based on specific rules or patterns
            // For demonstration, let's reverse the obfuscated name
            char[] chars = obfuscatedName.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }

        private string SimplifyControlFlow(string code)
        {
            code = Regex.Replace(code, @"if\s*\(condition1\)\s*{(?:(?!}).)*\s*}(\s*else\s*{(?:(?!}).)*\s*})?", match =>
            {
                string ifBlock = match.Groups[0].Value;
                string elseBlock = match.Groups[1].Value;
                if (!string.IsNullOrWhiteSpace(elseBlock))
                {
                    return $"if (condition1) {{{ifBlock}}} else {{{elseBlock}}}";
                }
                else
                {
                    return ifBlock;
                }
            });

            return code;
        }

        private string OptimizeLoops(string code)
        {
            // Optimize loops in the code if possible

            // Example: Replace a while loop with a for loop if the loop condition and increment can be expressed more efficiently
            code = Regex.Replace(code, @"while\s*\(i\s*<\s*n\)\s*{\s*.*?\s*i\s*=\s*i\s*\+\s*1\s*;\s*}", match =>
            {
                return "for (int i = 0; i < n; i++) {";
            });

            return code;
        }

        private string RemoveUnusedImports(string code)
        {
            // Remove unused import statements from the code

            // Example: Remove System.Diagnostics import statement if it's not used in the code
            if (!Regex.IsMatch(code, @"\bDebug\b"))
            {
                code = code.Replace("using System.Diagnostics;", "");
            }

            // Add more checks for other import statements as needed

            return code;
        }

        private string RestoreObfuscatedCodeStructure(string code)
        {
            // Restore the original structure of the obfuscated code if necessary
            // This method can be used to undo certain transformations that were performed during deobfuscation

            return code;
        }
    }
    class EntryPoint
    {
        static void Start(string[] args)
        {
            string filePath = "obfuscated_code.txt";
            Deobf deobfuscator = new Deobf(filePath);
            deobfuscator.StartDeobfuscation();
        }
    }
}