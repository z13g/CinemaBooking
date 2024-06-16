import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { FrontpageComponent } from './components/frontpage/frontpage.component';
import { AreapickComponent } from './components/areapick/areapick.component';
import { AdminFrontPageComponent } from './components/admin/admin-front-page/adminFrontPage.component';
import { AdminMovieComponent } from './components/admin/admin-movie/adminmovie.component';
import { authGuard, adminGuard } from './guard/auth.guard';
import { UnauthorizedComponent } from './components/unauthorized/unauthorized.component';
import { AdmingenreComponent } from './components/admin/admin-genre/admingenre.component';
import { AdminCinemaComponent } from './components/admin/admin-cinema/admincinema.component';
import { AdmincinemaHallComponent } from './components/admin/admincinema-hall/admincinemahall.component';
import { AdminregionComponent } from './components/admin/adminregion/adminregion.component';
import { AdminareaComponent } from './components/admin/adminarea/adminarea.component';
import { AdminroleComponent } from './components/admin/adminrole/adminrole.component';
import { AdminseatComponent } from './components/admin/adminseat/adminseat.component';
import { AdminshowComponent } from './components/admin/adminshow/adminshow.component';
import { AdminuserdetailComponent } from './components/admin/adminuserdetail/adminuserdetail.component';
import { MoviePageComponent } from './components/movie-page/movie-page.component';
import { BookingComponent } from './components/booking/booking.component';
import { NotfoundComponent } from './components/notfound/notfound.component';
import { PaymentComponent } from './components/payment/payment.component';
import { ConfirmationComponent } from './components/confirmation/confirmation.component';

const routes: Routes = [
  { path: '', component: FrontpageComponent},
  { path: 'profile', component: FrontpageComponent, canActivate : [authGuard]},
  { path: 'admin', component: AdminFrontPageComponent, canActivate : [adminGuard]},
  { path: 'login', component: LoginComponent },
  { path: 'areapick', component: AreapickComponent },
  { path: 'moviepage/:movieId', component: MoviePageComponent },
  { path: 'unauthorized', component: UnauthorizedComponent },
  { path: 'unauthorized', component: UnauthorizedComponent },
  { path: 'booking/:id', component: BookingComponent},
  { path: 'payment', component: PaymentComponent, canActivate : [authGuard]},
  { path: 'confirmation', component: ConfirmationComponent, canActivate : [authGuard]},
  { path: '404', component: NotfoundComponent },
  { path: '**', redirectTo: '/404' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
