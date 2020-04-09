open System
open Apache.NMS
open Apache.NMS.Util



 
  
  
let ConsumerTask (host_adrs : string, port : uint16, queue_name : string) =
    async {
        let connect_uri = ("activemq:tcp://{0}:{1}", host_adrs, port) |> String.Format |> Uri
        let factory = connect_uri |> NMSConnectionFactory

        use conn = factory.CreateConnection()
        use sess = conn.CreateSession()
        
        use dest = (sess, ("queue://{0}",queue_name) |> String.Format) |> SessionUtil.GetDestination
        use consumer = dest |> sess.CreateConsumer
        
        conn.Start()
        
        while true do 
            let message = consumer.Receive() :?> ITextMessage
            if message <> null then
                printfn "%s" (message.Text)
   }

    

[<EntryPoint>]
let main argv =
    ("127.0.0.1", 61616us, "TEST") |> ConsumerTask |> Async.RunSynchronously
    0 // return an integer exit code
