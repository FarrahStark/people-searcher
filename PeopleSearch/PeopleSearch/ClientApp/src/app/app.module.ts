import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { AddPeopleComponent } from './add-people/add-people.component';
import { PersonDetailsComponent } from './person-details/person-details.component';
import { ReactiveFormsModule } from '@angular/forms';
import {
  MatCardModule,
  MatTableModule,
  MatFormFieldModule,
  MatInputModule,
  MatProgressBarModule,
  MatIconModule,
  MatTooltipModule,
  MatSidenavModule,
  MatButtonModule
} from '@angular/material';
import { CommonModule } from '@angular/common';
import { PersonRepositoryService } from './services/person-repository.service';
import { DrawerService } from './services/drawer.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import 'hammerjs';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    AddPeopleComponent,
    PersonDetailsComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MatCardModule,
    MatTableModule,
    MatFormFieldModule,
    MatInputModule,
    MatProgressBarModule,
    MatIconModule,
    MatTooltipModule,
    MatSidenavModule,
    MatButtonModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'add-people', component: AddPeopleComponent },
    ])
  ],
  providers: [
    PersonRepositoryService,
    DrawerService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
