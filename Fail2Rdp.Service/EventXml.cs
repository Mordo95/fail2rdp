
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fail2Rdp.Service
{
    [XmlRoot(ElementName = "Provider")]
    public class Provider
    {
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "Guid")]
        public string Guid { get; set; }
    }

    [XmlRoot(ElementName = "TimeCreated")]
    public class TimeCreated
    {
        [XmlAttribute(AttributeName = "SystemTime")]
        public string SystemTime { get; set; }
    }

    [XmlRoot(ElementName = "Correlation")]
    public class Correlation
    {
        [XmlAttribute(AttributeName = "ActivityID")]
        public string ActivityID { get; set; }
    }

    [XmlRoot(ElementName = "Execution")]
    public class Execution
    {
        [XmlAttribute(AttributeName = "ProcessID")]
        public string ProcessID { get; set; }
        [XmlAttribute(AttributeName = "ThreadID")]
        public string ThreadID { get; set; }
    }

    [XmlRoot(ElementName = "System")]
    public class SystemXml
    {
        [XmlElement(ElementName = "Provider")]
        public Provider Provider { get; set; }
        [XmlElement(ElementName = "EventID")]
        public string EventID { get; set; }
        [XmlElement(ElementName = "Version")]
        public string Version { get; set; }
        [XmlElement(ElementName = "Level")]
        public string Level { get; set; }
        [XmlElement(ElementName = "Task")]
        public string Task { get; set; }
        [XmlElement(ElementName = "Opcode")]
        public string Opcode { get; set; }
        [XmlElement(ElementName = "Keywords")]
        public string Keywords { get; set; }
        [XmlElement(ElementName = "TimeCreated")]
        public TimeCreated TimeCreated { get; set; }
        [XmlElement(ElementName = "EventRecordID")]
        public string EventRecordID { get; set; }
        [XmlElement(ElementName = "Correlation")]
        public Correlation Correlation { get; set; }
        [XmlElement(ElementName = "Execution")]
        public Execution Execution { get; set; }
        [XmlElement(ElementName = "Channel")]
        public string Channel { get; set; }
        [XmlElement(ElementName = "Computer")]
        public string Computer { get; set; }
        [XmlElement(ElementName = "Security")]
        public string Security { get; set; }
    }

    [XmlRoot(ElementName = "Data")]
    public class EventXMLData
    {
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "EventData")]
    public class EventData
    {
        [XmlElement(ElementName = "Data")]
        public List<EventXMLData> Data { get; set; }

        public string this[string key]
        {
            get { return Data.FirstOrDefault(x => x.Name == key)?.Text; }
        }
    }

    [XmlRoot(ElementName = "Event")]
    public class EventXml
    {
        [XmlElement(ElementName = "System")]
        public SystemXml System { get; set; }
        [XmlElement(ElementName = "EventData")]
        public EventData EventData { get; set; }

        public static EventXml Parse(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(EventXml), "http://schemas.microsoft.com/win/2004/08/events/event");
            using (TextReader ms = new StringReader(xml))
                return serializer.Deserialize(ms) as EventXml;
        }
    }

}