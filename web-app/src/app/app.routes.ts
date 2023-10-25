import { Routes } from '@angular/router';
import { NotFoundComponent } from './not-found/not-found.component';
import { AuthComponent } from './auth/auth.component';
import { HomeComponent } from './home/home.component';
import { authGuard } from './core/guards/auth.guard';
import { AdminLayoutComponent } from './admin-layout/admin-layout.component';
import { SpecialityComponent } from './speciality/speciality.component';

export const routes: Routes = [
  {
    path: '',
    component: AuthComponent,
  },
  {
    path:'home',
    component: HomeComponent,
    //canActivate: [authGuard]
  },
  {
    path:'admin',
    component: AdminLayoutComponent,
    //canActivate: [authGuard],
    children: [
      {
        path: 'specialities',
        component: SpecialityComponent,
      }
    ]
  },
  {
    path:'**',
    component: NotFoundComponent,
  }
];
