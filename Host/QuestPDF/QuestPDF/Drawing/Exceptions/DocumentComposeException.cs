﻿using System;

namespace QuestPDF.Drawing.Exceptions
{
    public class DocumentComposeException : Exception
    {
        public DocumentComposeException()
        {
        }

        public DocumentComposeException(
            string message
        ) : base(message: message)
        {
        }

        public DocumentComposeException(
            string message
            , Exception inner
        ) : base(message: message, innerException: inner)
        {
        }
    }
}