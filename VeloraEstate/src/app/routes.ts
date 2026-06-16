import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { PropertyDetailsComponent } from './property-details/property-details.component';
import { AuthComponent } from './auth/auth.component';
import { AdminComponent } from './admin/admin.component';
import { AboutComponent } from './about/about.component';
import { ContactPageComponent } from './contact-page/contact-page.component';
import { authGuard } from './auth.guard';
import { TermsComponent } from './terms/terms.component';
import { PrivacyPolicyComponent } from './privacy-policy/privacy-policy.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';
import { ForbiddenComponent } from './forbidden/forbidden.component';
import { ServerErrorComponent } from './server-error/server-error.component';
import { ProfileComponent } from './profile/profile.component';

const routeConfig: Routes = [
  {
    path: '',
    component: HomeComponent,
    title: 'VeloraEstate — Premium Properties'
  },
  {
    path: 'property/:id',
    component: PropertyDetailsComponent,
    title: 'Property Details'
  },
  {
    path: 'login',
    component: AuthComponent,
    title: 'Sign In — VeloraEstate'
  },
  {
    path: 'admin',
    component: AdminComponent,
    title: 'Admin Panel',
    canActivate: [authGuard]
  },
  {
    path: 'about',
    component: AboutComponent,
    title: 'About Us — VeloraEstate'
  },
  {
    path: 'contact',
    component: ContactPageComponent,
    title: 'Contact — VeloraEstate'
  },
  {
  path: 'terms',
  component: TermsComponent,
  title: 'Terms & Conditions — VeloraEstate'
},
{
  path: 'privacy',
  component: PrivacyPolicyComponent,
  title: 'Privacy Policy — VeloraEstate'
},
{ 
  path: 'profile', 
  component: ProfileComponent, 
  title: 'My Profile — VeloraEstate' 
},

{
  path: '401',
  component: UnauthorizedComponent,
  title: '401 — VeloraEstate'
},
{
  path: '403',
  component: ForbiddenComponent,
  title: '403 — VeloraEstate'
},
{
  path: '500',
  component: ServerErrorComponent,
  title: '500 — VeloraEstate'
},
{
  path: '**',
  component: NotFoundComponent,
  title: '404 — VeloraEstate'
}

];

export default routeConfig;