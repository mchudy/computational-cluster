﻿using ComputationalCluster.Common.Objects;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ComputationalCluster.Common.Messages
{
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [XmlType(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    [XmlRoot(Namespace = "http://www.mini.pw.edu.pl/ucc/", IsNullable = false, ElementName = "NoOperation")]
    public class NoOperationMessage
    {
        public IList<BackupCommunicationServer> BackupCommunicationServersList { get; set; }
    }
}
