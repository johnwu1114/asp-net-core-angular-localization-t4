import { Component } from "@angular/core";
@Component({
    selector: "my-app",
    templateUrl: "/app/app.component.html"
})
export class AppComponent {
    login = R.Text.Login;
    password = R.Text.Password;
    username = R.Text.UserName;
    send = R.Text.Send;
    internalServerError = R.Message.InternalServerError;
}