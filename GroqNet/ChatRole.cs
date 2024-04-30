﻿namespace GroqNet
{
    /// <summary>
    /// Defines the roles that can be assigned to messages in a chat environment.
    /// </summary>
    public enum ChatRole
    {
        /// <summary>
        /// Represents a system message which is used to set the behavior of the assistant.
        /// </summary>
        System,

        /// <summary>
        /// Represents a message from a user interacting with the language model.
        /// </summary>
        User,

        /// <summary>
        /// Represents a message generated by the assistant in response to a user message.
        /// </summary>
        Assistant
    }
}