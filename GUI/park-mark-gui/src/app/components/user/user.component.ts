import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user.model';
import { UserService } from 'src/app/services/user.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent  {
  newUser:User = new User();
  constructor(private userService:UserService) { }


  SignUp(frm:any){
    console.log(this.newUser.code,this.newUser.username,this.newUser.userpassword);
    this.userService.SignUp(this.newUser).subscribe(code=>{
     //לקבל את הקוד חברה שנכנס עכשיו ולשלוח אותו להוספת בחירה
     this.newUser.code=code; 
     if(code!=0)
      {
        console.log("user has been added successfully")
       /* sessionStorage.setItem('companyId',userpassword)
        this.router.navigate(['/Election']);*/
      }
     else 
     console.log("בחר שם אחר שם משתמש זה כבר קיים במערכת")
     });
     
    }

}
