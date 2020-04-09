open Apache.NMS
open Apache.NMS.Util
open System





let ProducerTask (host_adrs : string, port : uint16, queue_name : string) =
    async {
        let connect_uri = ("activemq:tcp://{0}:{1}", host_adrs, port) |> String.Format |> Uri
        let factory = connect_uri |> NMSConnectionFactory

        use conn = factory.CreateConnection()
        use sess = conn.CreateSession()
        
        use dest = (sess, ("queue://{0}",queue_name) |> String.Format) |> SessionUtil.GetDestination
        use producer = dest |> sess.CreateProducer
        
        conn.Start()
        producer.DeliveryMode <- MsgDeliveryMode.Persistent
        producer.RequestTimeout <- (10.0 |> TimeSpan.FromSeconds)
        
        for i in 0 .. 99 do
            let msg = ("Hello world ... {0}", i) |> String.Format
            let request = sess.CreateTextMessage(msg)
            request |> producer.Send
   }    
    
    
    
    

[<EntryPoint>]
let main argv =
    ("127.0.0.1", 61616us, "TEST") |> ProducerTask |> Async.RunSynchronously    
    0 // return an integer exit code
