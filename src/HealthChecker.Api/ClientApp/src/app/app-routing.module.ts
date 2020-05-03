import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HealthCheckComponent } from './pages';
import { PATH } from './configs';

const appRoutes: Routes = [
    { path: '', redirectTo: PATH.HEALTH_CHECK, pathMatch: 'full' },
    { path: PATH.HEALTH_CHECK, component: HealthCheckComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(appRoutes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
