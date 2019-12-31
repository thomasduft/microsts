import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { Routes, RouterModule } from '@angular/router';

import { OAuthModule } from 'angular-oauth2-oidc';

import { SharedModule } from './shared/shared.module';
import { ForbiddenComponent, PageNotFoundComponent, AuthGuard } from './shared';

import { HomeModule } from './home/home.module';
import { SecretModule } from './secret/secret.module';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home/home.component';
import { SecretComponent } from './secret/secret/secret.component';

const ROUTES: Routes = [
  { path: 'home', component: HomeComponent },
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'secret', component: SecretComponent, canActivate: [AuthGuard] },
  { path: 'forbidden', component: ForbiddenComponent },
  { path: '**', component: PageNotFoundComponent }
];

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    RouterModule.forRoot(ROUTES),
    OAuthModule.forRoot(),
    SharedModule,
    HomeModule,
    SecretModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
