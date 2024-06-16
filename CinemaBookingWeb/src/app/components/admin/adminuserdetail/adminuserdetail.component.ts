import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { RegisterUserDTO } from 'src/app/models/registeruserdto/registeruserdto';
import { Role } from 'src/app/models/role/role';
import { Userdetail } from 'src/app/models/userdetail/userdetail';
import { GenericService } from 'src/app/services/generic.services';

@Component({
  selector: 'app-adminuserdetail',
  templateUrl: './adminuserdetail.component.html',
  styleUrls: ['../../../../static/assets/css/admin.css']
})
export class AdminuserdetailComponent {
  userDetailForm!: FormGroup;
  roleList: Role[] =[];
  userDetailList: Userdetail[] = [];
  currentUserDetail: Userdetail | null = null;
  isEditMode: boolean = false;
  showForm: boolean = false;
  showList: boolean = true;

  initForm(userdetail?: Userdetail): void {
    this.userDetailForm = new FormGroup({
      userDetailID: new FormControl(userdetail ? userdetail.userDetailID : 0),
      name: new FormControl(userdetail ? userdetail.name : '', Validators.required),
      email: new FormControl(userdetail ? userdetail.email : '', Validators.required),
      password: new FormControl({value: userdetail && this.isEditMode ? userdetail.PasswordHash : '', disabled: this.isEditMode}, this.isEditMode ? null : Validators.required),
      phoneNumber: new FormControl(userdetail ? userdetail.phoneNumber : '', Validators.required),
      roleID: new FormControl(userdetail ? userdetail.roleID : '', Validators.required),
      isActive: new FormControl(userdetail ? userdetail.isActive : true, Validators.required),
    });
  }
  
  constructor(private userDetailService: GenericService<Userdetail>, private roleSerice: GenericService<Role>) {
    this.initForm();
  }
  ngOnInit() {
    this.fetchUserDetail();
    this.initForm();

    this.userDetailForm.get('isActive')!.valueChanges.subscribe(active => {
        console.log("isActive toggled, new value:", active);
        if (this.isEditMode && this.currentUserDetail) {
            this.save();
        }
    });
}

  
    fetchUserDetail() {
      this.userDetailService.getAll("userDetail").subscribe(data => {
        this.userDetailList = data;
      });
  
      this.roleSerice.getAll("role").subscribe(data => {
        this.roleList = data;
      });
    }
  
    editUserDetail(userdetail: Userdetail): void {
      this.currentUserDetail = userdetail;
      console.log("CurrentUser: ", this.currentUserDetail);
      this.isEditMode = true;
      this.showForm = true;
      this.showList = false;
      this.initForm(userdetail);
    }
  
    resetForm(): void {
      this.userDetailForm.reset();
      this.showForm = false;
      this.showList = true;
      this.isEditMode = false;
      this.currentUserDetail = null;
    }
  
    toggleSave(): void {
      this.isEditMode = false;
      this.currentUserDetail = null;
      this.initForm();
      this.showForm = !this.showForm;
      this.showList = !this.showForm;
    }
  
    public save(): void {
      console.log("Attempting to save. Form valid:", this.userDetailForm.valid);
      console.log("Userdetail: ", this.userDetailForm.value)
      if (this.userDetailForm.valid) {
        const formdata = this.userDetailForm.value;
        console.log("Formdata: ", formdata);
        
        const registerData: RegisterUserDTO = {
          name: formdata.name,
          email: formdata.email,
          phoneNumber: formdata.phoneNumber,
          password: formdata.password
        };
  
        const userDetailDTOData = {
          userDetailID: this.isEditMode ? formdata.userDetailID : 0,
          name: formdata.name,
          email: formdata.email,
          phoneNumber: formdata.phoneNumber,
          roleID: parseInt(formdata.roleID, 10),
          isActive: formdata.isActive
        };

        console.log("TestID", userDetailDTOData.userDetailID);
        
        if (this.isEditMode == true && userDetailDTOData.userDetailID) {  
          console.log("UpdateTest", userDetailDTOData);
          
          this.userDetailService.update('userdetail', userDetailDTOData, userDetailDTOData.userDetailID).subscribe({
            next: (response) => {
              console.log('UserDetail updated:', response);
              this.resetForm();
              this.fetchUserDetail();
            },
            error: (error) => {
              console.error('Failed to update UserDetail:', error);
              alert(`Failed to update UserDetail: ${error.error.title}`);
            }
          });
        }
           else
           {
            if (userDetailDTOData.roleID == 1)
              {
                console.log("UserDetailCostumer Create: ", registerData);
            
              this.userDetailService.create('UserDetail/registerCostumerOutToken', registerData).subscribe({
                next: (response) => {
                  console.log('UserdetailCostumer saved:', response);
                  this.userDetailList.push(response);
                  this.userDetailForm.reset();
                  this.showForm = false;
                  this.showList = true;
                },
                error: (error) => {
                  alert(`Failed to create userDetail: ${error.error.title}`);
                }
              });
              }
            else if (userDetailDTOData.roleID == 2)
              {
                console.log("UserDetailAdmin Create: ", registerData);
            
              this.userDetailService.create('UserDetail/registerAdminOutToken', registerData).subscribe({
                next: (response) => {
                  console.log('UserdetailCostumer saved:', response);
                  this.userDetailList.push(response);
                  this.userDetailForm.reset();
                  this.showForm = false;
                  this.showList = true;
                },
                error: (error) => {
                  alert(`Failed to create userDetail: ${error.error.title}`);
                }
              });
              }
        
      }
    }
   }
    
    deleteUserDetail(userdetail: Userdetail) {
      if (userdetail && userdetail.userDetailID !== undefined) {
          this.userDetailService.delete('userdetail', userdetail.userDetailID).subscribe(() => {
              this.fetchUserDetail();
          });
      }
    }
  }
