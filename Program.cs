// Telegram open close update position message by paolo panicali july 2022

using System;
using System.Linq;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using cAlgo.Indicators;
using System.IO;
using System.Net;
using System.Text;

namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.Internet)]
    public class July19TelegramOpenCloseUpdate : Robot
    {
        string text, urlString, apiToken, chatId;

        protected override void OnStart()
        {
            Positions.Opened += PositionsOnOpened;
            Positions.Closed += PositionsOnClosed;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            apiToken = "apitoken";
            chatId = "chatid";
        }

        protected override void OnBar()
        {
            if (Bars.OpenTimes.LastValue.Minute == 5)
            {
                SendMessageToTelegram();
            }
        }

        // send to telegram class, takes one argument, the text. Obviously the other arguments should be the chat id and maybe the bot token if using various
        protected void SendMessageToTelegram()
        {
            foreach (var position in Positions)
            {
                text = "UPDATE CURRENTLY OPEN POSITION : ////--- BOTNAME: " + position.Label + "--- Symbol: " + position.SymbolName + "---- ENTRY: " + position.EntryPrice + "---- SL: " + position.StopLoss + "---- TP: " + position.TakeProfit + "---- TIME UTC: " + position.EntryTime + "---- PROFIT:  " + position.Pips + " pips";
                urlString = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}";
                urlString = String.Format(urlString, apiToken, chatId, text);
                WebRequest request = WebRequest.Create(urlString);
                Stream rs = request.GetResponse().GetResponseStream();
                System.Threading.Thread.Sleep(5000);
            }
        }



        private void PositionsOnOpened(PositionOpenedEventArgs args)
        {
            Print("Position opened {0}", args.Position.SymbolName);

            text = "NEW TRADE OPENED : ////--- BOTNAME: " + args.Position.Label + "--- Symbol: " + args.Position.SymbolName + "---- ENTRY: " + args.Position.EntryPrice + "---- SL: " + args.Position.StopLoss + "---- TP: " + args.Position.TakeProfit + "---- TIME UTC: " + args.Position.EntryTime + "---- PROFIT:  " + args.Position.Pips + " pips";
            urlString = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}";
            urlString = String.Format(urlString, apiToken, chatId, text);
            WebRequest request = WebRequest.Create(urlString);
            Stream rs = request.GetResponse().GetResponseStream();
        }

        private void PositionsOnClosed(PositionClosedEventArgs args)
        {
            var position = args.Position;
            Print("Position closed with {0} profit", position.SymbolName);

            text = "TRADE CLOSED : ////--- BOTNAME: " + args.Position.Label + "--- Symbol: " + args.Position.SymbolName + "---- ENTRY: " + args.Position.EntryPrice + "---- SL: " + args.Position.StopLoss + "---- TP: " + args.Position.TakeProfit + "---- TIME UTC: " + args.Position.EntryTime + "---- PROFIT:  " + args.Position.Pips + " pips";
            urlString = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}";
            urlString = String.Format(urlString, apiToken, chatId, text);
            WebRequest request = WebRequest.Create(urlString);
            Stream rs = request.GetResponse().GetResponseStream();

        }


        protected override void OnStop()
        {
            // Put your deinitialization logic here
        }
    }
}
