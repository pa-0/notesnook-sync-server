/*
This file is part of the Notesnook Sync Server project (https://notesnook.com/)

Copyright (C) 2022 Streetwriters (Private) Limited

This program is free software: you can redistribute it and/or modify
it under the terms of the Affero GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
Affero GNU General Public License for more details.

You should have received a copy of the Affero GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Streetwriters.Common.Enums;
using Streetwriters.Common.Interfaces;
using Streetwriters.Common.Models;

namespace Streetwriters.Common
{
    public class Slogger<T>
    {
        public static Task Info(string scope, params string[] messages)
        {
            return Write(Format("info", scope, messages));
        }

        public static Task Error(string scope, params string[] messages)
        {
            return Write(Format("error", scope, messages));
        }
        private static string Format(string level, string scope, params string[] messages)
        {
            var date = DateTime.UtcNow.ToString("MM-dd-yyyy HH:mm:ss");
            var messageText = string.Join(" ", messages);
            return $"[{date}] | {level} | <{scope}> {messageText}";
        }
        private static Task Write(string line)
        {
            var logDirectory = Path.GetFullPath("./logs");
            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);
            var path = Path.Join(logDirectory, typeof(T).FullName + "-" + DateTime.UtcNow.ToString("MM-dd-yyyy") + ".log");
            return File.AppendAllLinesAsync(path, new string[1] { line });
        }
    }
}