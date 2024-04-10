import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './views/home/home.component';
import { AuthComponent } from './views/auth/auth.component';
import { LoginComponent } from './views/auth/login/login.component';
import { JwtModule, JwtHelperService } from '@auth0/angular-jwt';
import { AuthService, authFactory } from './services/auth.service';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RegisterComponent } from './views/auth/register/register.component';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ServerService } from './services/server.service';
import { ToString } from './pipes/parseId.pipe';
import { ServerComponent } from './views/server/server.component';
import { ModalService } from './services/modal.service';
import { NewServerModalComponent } from './components/new-server-modal/new-server-modal.component';
import { DropdownComponent } from './components/dropdown/dropdown.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AuthComponent,
    LoginComponent,
    RegisterComponent,
    NavBarComponent,
    ServerComponent,
    NewServerModalComponent,
    DropdownComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToString,
    JwtModule.forRoot({
      config: {
        tokenGetter: () => localStorage.getItem('token') || '',
        allowedDomains: ["localhost"],
      }
    })
  ],
  providers: [
    AuthService,
    ServerService,
    ModalService,
    JwtHelperService,
    {
      provide: APP_INITIALIZER,
      useFactory: authFactory,
      multi: true,
      deps: [AuthService],
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
