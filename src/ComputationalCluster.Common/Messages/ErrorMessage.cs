﻿using System;
using System.Xml.Serialization;

namespace ComputationalCluster.Common.Messages
{
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [XmlType(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    [XmlRoot(Namespace = "http://www.mini.pw.edu.pl/ucc/", IsNullable = false, ElementName = "Error")]
    public class ErrMessage : Message
    {
        public ErrorErrorType ErrorType { get; set; }

        public string ErrorMessage { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    public enum ErrorErrorType
    {
        UnknownSender,
        InvalidOperation,
        ExceptionOccured,
        NotAPrimaryServer
    }
}
