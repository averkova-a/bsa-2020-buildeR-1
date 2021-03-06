import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { SignUpComponent } from '@core/components/sign-up/sign-up.component';
import { SignInComponent } from '@core/components/sign-in/sign-in.component';
import { HomeGuard } from '@core/guards/home.guard';
import { AuthResolver } from '@core/resolvers/auth.resolver';
import { RawLogsComponent } from '@core/components/raw-logs/raw-logs.component';

const routes: Routes = [
  { path: '',
    loadChildren: () =>
      import('./modules/landing-page/landing-page.module')
        .then(m => m.LandingPageModule),
    canActivate: [HomeGuard]
  },
  { path: 'signin', component: SignInComponent, canActivate: [HomeGuard] },
  { path: 'signup', component: SignUpComponent, canActivate: [HomeGuard] },
  {
    path: 'portal',
    canActivate: [AuthGuard],
    resolve: {
      auth: AuthResolver
    },
    loadChildren: () => import('./modules/work-space/work-space.module')
      .then(m => m.WorkSpaceModule),
  },
  { path: 'logs', component: RawLogsComponent},
  { path: '**', redirectTo: '', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
