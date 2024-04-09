import { NgModule, inject } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './views/home/home.component';
import { AuthComponent } from './views/auth/auth.component';
import { LoginComponent } from './views/auth/login/login.component';
import { AuthGuard } from './services/auth.guard';
import { LoginResolve } from './services/login.resolve';
import { RegisterComponent } from './views/auth/register/register.component';
import { ServerComponent } from './views/server/server.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    canActivate: [() => inject(AuthGuard).canActivate()],
    data: { animation: 'home' }
  },
  {
    path: 'server',
    canActivate: [() => inject(AuthGuard).canActivate()],
    children: [
      {
        path: '',
        redirectTo: '/',
        pathMatch: 'full'
      },
      {
        path: ':id',
        component: ServerComponent,
        data: { animation: 'server-details' }
      },
    ]
  },
  {
    path: '',
    component: AuthComponent,
    children: [
      {
        path: 'login',
        component: LoginComponent,
        data: { animation: 'login' },
        resolve: {
          ready: () => inject(LoginResolve).resolve()
        }
      },
      {
        path: 'register',
        component: RegisterComponent,
        data: { animation: 'register' },
        resolve: {
          ready: () => inject(LoginResolve).resolve()
        }
      },
    ]
  },
  {
    path: '**',
    redirectTo: '',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
