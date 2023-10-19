import { Routes } from '@angular/router';
import { NotFoundComponent } from './components/not-found-componet/not-found.component';
import { AuthComponent } from './auth/auth.component';
import { HomeComponent } from './home/home.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    component: AuthComponent,
  },
  {
    path:'home',
    component: HomeComponent,
    canActivate: [authGuard]
  },
  {
    path:'**',
    component: NotFoundComponent,
  }
];
