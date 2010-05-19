/*
  This file is licensed to You under the Apache License, Version 2.0
  (the "License"); you may not use this file except in compliance with
  the License.  You may obtain a copy of the License at

  http://www.apache.org/licenses/LICENSE-2.0

  Unless required by applicable law or agreed to in writing, software
  distributed under the License is distributed on an "AS IS" BASIS,
  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
  See the License for the specific language governing permissions and
  limitations under the License.
*/

using System.IO;
using System.Xml;

namespace net.sf.xmlunit.builder {

    public sealed class Transform {

        public interface IBuilder : ITransformationBuilderBase<IBuilder> {
            ITransformationResult Build();
        }
        public interface ITransformationResult {
            void To(Stream s);
            void To(TextWriter t);
            void To(XmlWriter x);
            string ToString();
            XmlDocument ToDocument();
        }


        internal class TransformationBuilder
            : AbstractTransformationBuilder<IBuilder>, IBuilder,
              ITransformationResult {

            internal TransformationBuilder(ISource s) : base(s) {
            }
            public ITransformationResult Build() {
                return this;
            }
            public override string ToString() {
                return Helper.TransformToString();
            }
            public XmlDocument ToDocument() {
                return Helper.TransformToDocument();
            }
            public void To(Stream s) {
                Helper.TransformTo(s);
            }
            public void To(TextWriter w) {
                Helper.TransformTo(w);
            }
            public void To(XmlWriter w) {
                Helper.TransformTo(w);
            }
        }

        public static IBuilder Source(ISource s) {
            return new TransformationBuilder(s);
        }
    }
}