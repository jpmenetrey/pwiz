/*
 * Original author: Nicholas Shulman <nicksh .at. u.washington.edu>,
 *                  MacCoss Lab, Department of Genome Sciences, UW
 *
 * Copyright 2020 University of Washington - Seattle, WA
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using pwiz.Common.Collections;
using pwiz.Common.DataBinding;
using pwiz.Skyline.Util.Extensions;

namespace pwiz.Skyline.Model.Crosslinking
{
    public class ModificationSite : IComparable<ModificationSite>
    {
        public ModificationSite(int indexAa, string modName)
        {
            IndexAa = indexAa;
            ModName = modName;
        }

        public int IndexAa { get; private set; }
        public string ModName { get; private set; }

        protected bool Equals(ModificationSite other)
        {
            return IndexAa == other.IndexAa && ModName == other.ModName;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ModificationSite) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (IndexAa * 397) ^ (ModName != null ? ModName.GetHashCode() : 0);
            }
        }

        public int CompareTo(ModificationSite other)
        {
            if (other == null)
            {
                return 1;
            }

            int result = IndexAa.CompareTo(other.IndexAa);
            if (result == 0)
            {
                result = StringComparer.Ordinal.Compare(ModName, other.ModName);
            }

            return result;
        }

        public override string ToString()
        {
            return (IndexAa + 1) + @":" + ModName;
        }

        public static ModificationSite Parse(string value)
        {
            int ichColon = value.IndexOf(':');
            if (ichColon < 0)
            {
                throw new FormatException();
            }
            return new ModificationSite(int.Parse(value.Substring(0, ichColon)), value.Substring(ichColon + 1));
        }

        public static string ListToString(IEnumerable<ModificationSite> sites)
        {
            if (sites == null)
            {
                return null;
            }

            return string.Join(@",", sites.Select(site => DsvWriter.ToDsvField(',', site.ToString())));
        }

        public static IEnumerable<ModificationSite> ParseList(string listString)
        {
            if (listString == null)
            {
                return null;
            }
            if (string.IsNullOrEmpty(listString))
            {
                return ImmutableList<ModificationSite>.EMPTY;
            }
            var fields = new CsvFileReader(new StringReader(listString), false).ReadLine();
            return fields.Select(ModificationSite.Parse);
        }
    }
}
