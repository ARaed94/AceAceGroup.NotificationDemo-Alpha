import { Component, OnInit } from '@angular/core';
import { SwPush } from '@angular/service-worker';
import { BaseHttpService } from 'src/features/core/base/base.http.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  public subscribers: any[] = [];
  public optInBtnTitle: string;
  public Msg: string;

  public serverPrivateKey;
  public serverPublicKey;

  constructor(private swPush: SwPush, private http: BaseHttpService) {
  }

  ngOnInit() {
    this.getServerKeys();

    this.getSubscribers();

    if (Notification.permission !== "granted") {
      this.optInBtnTitle = "Opt In";
    }
    else {
      this.optInBtnTitle = "Opted In";
    }
  }

  send() {
    this.http.get("settings/send/" + this.Msg).subscribe((response: any) => {
      this.serverPublicKey = response.body.find(a => a.key == "PublicKey").value;
      this.serverPrivateKey = response.body.find(a => a.key == "PrivateKey").value;
    });
  }

  getServerKeys() {
    this.http.get("settings").subscribe((response: any) => {
      this.serverPublicKey = response.body.find(a => a.key == "PublicKey").value;
      this.serverPrivateKey = response.body.find(a => a.key == "PrivateKey").value;
    });
  }

  public getSubscribers(): void {
    this.http.get("subscribers").subscribe((response: any) => {
      this.subscribers = response.body || [];
    }, (error: any) => {
      console.log(error);
    });
  }

  optin() {
    if (this.serverPublicKey == null) {
      return;
    }

    if (Notification.permission !== "granted") {
      this.optInBtnTitle = "Opt In";

      this.swPush.requestSubscription({
        serverPublicKey: this.serverPublicKey
      })
        .then((sub: PushSubscription) => {
          console.log("update 2");
          console.log(sub.endpoint);
          let keys = {
            p256dh: sub.toJSON().keys.p256dh,
            auth: sub.toJSON().keys.auth
          };
          console.log(keys);


          let subscriber = {
            "Endpoint": sub.endpoint,
            "P256DH": keys.p256dh,
            "Auth": keys.auth
          };

          this.http.post("subscribers", subscriber).subscribe((result: any) => {
            this.getSubscribers();
          });

        })
        .catch(err => console.error("Could not subscribe to notifications", err));
    }
  }

  public testNotification() {
    console.log("update 1");
    debugger;
    this.swPush.requestSubscription({
      serverPublicKey: "BJdErwJmwoVV3kZ0liZ9BVimTnKIXK57Zh7FW6X9CFHnXnD9wyV4D2uBR2krZzYHiALGG3dRJmCnpbRsTt3iM1M "
    })
      .then((sub: PushSubscription) => {
        console.log("update 2");
        console.log(sub.endpoint);
        let keys = {
          p256dh: sub.toJSON().keys.p256dh,
          auth: sub.toJSON().keys.auth
        };
        console.log(keys)
      })
      .catch(err => console.error("Could not subscribe to notifications", err));
  }
}