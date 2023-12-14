import { Routes } from '@angular/router';
import { NotFoundComponent } from './not-found/not-found.component';
import { AuthComponent } from './auth/auth.component';
import { HomeComponent } from './home/home.component';
import { authGuard } from './core/guards/auth.guard';
import { LayoutComponent } from './admin/layout/layout.component';
import { SpecialityComponent } from './admin/speciality/speciality.component';
import { DoctorComponent } from './admin/doctor/doctor.component';
import { HistoryComponent } from './history/history.component';

export const routes: Routes = [
  {
    path: '',
    component: AuthComponent,
  },
  {
    path:'home',
    title: 'NC | Home',
    component: HomeComponent,
    canActivate: [authGuard]
  },
  {
    path: 'history/:id',
    title: 'NC | Patient history',
    component: HistoryComponent,
    canActivate: [authGuard]
  },
  {
    path:'admin',
    component: LayoutComponent,
    canActivate: [authGuard],
    children: [
      {
        path:'',
        title: 'NC | Admin',
        redirectTo: 'doctors',
        pathMatch: 'full',
      },
      {
        path: 'specialities',
        title: 'NC | Admin - Specialties',
        component: SpecialityComponent,
      },
      {
        path: 'doctors',
        title: 'NC | Admin - Doctors',
        component: DoctorComponent,
      },
    ]
  },
  {
    path:'**',
    component: NotFoundComponent,
  }
];
