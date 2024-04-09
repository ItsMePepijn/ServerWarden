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

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AuthComponent,
    LoginComponent,
    RegisterComponent,
    NavBarComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: () => localStorage.getItem('token') || '',
        allowedDomains: ["localhost"],
      }
    })
  ],
  providers: [
    AuthService,
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
