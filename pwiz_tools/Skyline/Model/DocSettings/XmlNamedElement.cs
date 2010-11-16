/*
 * Original author: Brendan MacLean <brendanx .at. u.washington.edu>,
 *                  MacCoss Lab, Department of Genome Sciences, UW
 *
 * Copyright 2009 University of Washington - Seattle, WA
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
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using pwiz.Skyline.Util;

namespace pwiz.Skyline.Model.DocSettings
{
    /// <summary>
    /// Base class for use with elements to be stored in
    /// <see cref="XmlMappedList{TKey,TValue}"/>.
    /// 
    /// This does not derive from <see cref="NamedElement"/>, because
    /// the single <see cref="Name"/> property in both cases should
    /// be read-only to preserve immutability of derrived types.
    /// </summary>
    public abstract class XmlNamedElement : Immutable, IKeyContainer<string>, IXmlSerializable
    {
        /// <summary>
        /// Parameterless constructor for serialization use only.
        /// </summary>
        protected XmlNamedElement()
        {
        }

        protected XmlNamedElement(string name)
        {
            Name = name;

            Validate();
        }

        protected XmlNamedElement(XmlNamedElement source)
            : this(source.Name)
        {
        }

        public string Name { get; private set; }

        public virtual string GetKey()
        {
            return Name;
        }

        #region Property change methods

        public XmlNamedElement ChangeName(string prop)
        {
            return ChangeProp(ImClone(this), (im, v) => im.Name = v, prop);
        }
        
        #endregion

        #region Implementation of IXmlSerializable

        private enum ATTR { name }

        private void Validate()
        {
            if (string.IsNullOrEmpty(Name))
                throw new InvalidDataException("Name property may not be missing or empty.");
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Reads the "name" attribute from the XML element for this
        /// named instance.  Overrides should call this base method
        /// before reading sub-elements, since the name is written
        /// as an attribute.
        /// </summary>
        /// <param name="reader">The XML reader from which the element is being read</param>
        public virtual void ReadXml(XmlReader reader)
        {
            Name = reader.GetAttribute(ATTR.name);

            Validate();
        }

        /// <summary>
        /// Writes the "name" attribute to the XML element for this
        /// named instance.  Overrides should call this base method
        /// before writing anything else, to make the name the first
        /// attribute in the XML element.
        /// </summary>
        /// <param name="writer">The XML writer to which the element is being written</param>
        public virtual void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString(ATTR.name, Name);
        }

        #endregion

        #region object overrides

        public bool Equals(XmlNamedElement obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj.Name, Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(XmlNamedElement)) return false;
            return Equals((XmlNamedElement)obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        #endregion
    }

    public abstract class XmlNamedIdElement : XmlNamedElement
    {
        protected XmlNamedIdElement(Identity id, string name) : base(name)
        {
            Id = id;
        }

        protected XmlNamedIdElement(Identity id)
        {
            Id = id;
        }

        public Identity Id { get; private set; }
    }

    public sealed class NameComparer<TElem> : IEqualityComparer<TElem>
        where TElem : XmlNamedElement
    {
        public bool Equals(TElem n1, TElem n2)
        {
            return Equals(n1.Name, n2.Name);
        }

        public int GetHashCode(TElem n)
        {
            return n.Name.GetHashCode();
        }
    }
}