using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

public class TCP
{
    public static string TraverseStates(string[] events)
    {
        var state = State.CLOSED;
        string s = "CLOSED";// initial state, always
                            // Your code here!

        for (int i = 0; i < events.Length; i++)
        {
            state = GetNextState(state, Enum.Parse<Signal>(events[i]));
        }
        return state.ToString();
    }
    static State GetNextState(State current, Signal signal)
    {
        return (current, signal) switch
        {
            (State.CLOSED, Signal.APP_PASSIVE_OPEN) => State.LISTEN,
            (State.CLOSED, Signal.APP_ACTIVE_OPEN) => State.SYN_SENT,
            (State.LISTEN, Signal.RCV_SYN) => State.SYN_RCVD,
            (State.LISTEN, Signal.APP_SEND) => State.SYN_SENT,
            (State.LISTEN, Signal.APP_CLOSE) => State.CLOSED,
            (State.SYN_RCVD, Signal.APP_CLOSE) => State.FIN_WAIT_1,
            (State.SYN_RCVD, Signal.RCV_ACK) => State.ESTABLISHED,
            (State.SYN_SENT, Signal.RCV_SYN) => State.SYN_RCVD,
            (State.SYN_SENT, Signal.RCV_SYN_ACK) => State.ESTABLISHED,
            (State.SYN_SENT, Signal.APP_CLOSE) => State.CLOSED,
            (State.ESTABLISHED, Signal.APP_CLOSE) => State.FIN_WAIT_1,
            (State.ESTABLISHED, Signal.RCV_FIN) => State.CLOSE_WAIT,
            (State.FIN_WAIT_1, Signal.RCV_FIN) => State.CLOSING,
            (State.FIN_WAIT_1, Signal.RCV_FIN_ACK) => State.TIME_WAIT,
            (State.FIN_WAIT_1, Signal.RCV_ACK) => State.FIN_WAIT_2,
            (State.CLOSING, Signal.RCV_ACK) => State.TIME_WAIT,
            (State.FIN_WAIT_2, Signal.RCV_FIN) => State.TIME_WAIT,
            (State.TIME_WAIT, Signal.APP_TIMEOUT) => State.CLOSED,
            (State.CLOSE_WAIT, Signal.APP_CLOSE) => State.LAST_ACK,
            (State.LAST_ACK, Signal.RCV_ACK) => State.CLOSED,
            _ => State.ERROR
        };
    }
    public enum State
    {
        CLOSED,
        LISTEN,
        SYN_SENT,
        SYN_RCVD,
        ESTABLISHED,
        CLOSE_WAIT,
        LAST_ACK,
        FIN_WAIT_1,
        FIN_WAIT_2,
        CLOSING,
        TIME_WAIT,
        ERROR

    }
    public enum Signal
    {
        APP_PASSIVE_OPEN,
        APP_ACTIVE_OPEN,
        APP_SEND,
        APP_CLOSE,
        APP_TIMEOUT,
        RCV_SYN,
        RCV_ACK,
        RCV_SYN_ACK,
        RCV_FIN,
        RCV_FIN_ACK
    }
}