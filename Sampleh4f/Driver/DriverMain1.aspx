<%@ Page Title="" Language="vb" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="DriverMain1.aspx.vb" Inherits="Sampleh4f.DriverMain1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div ng-app="hungry4foodApp" ng-controller="driverCtrl">

        <%--<div class="main-container container main-body-border" ng-init="OnInit()">--%>
        <div class="main-container row" ng-form="employeeform" ng-init="OnInit()">

            <div class="col-sm-10">
                <%-- {{ServerResponse}}--%>
                <div class="panel panel-default main-form">
                    <div class="panel-heading">Employee Details</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-sm-3">

                                <md-input-container flex>
             <label for="empFirstName">First Name</label>
          <input type="text" name="name" md-maxlength="30"   ng-model="employee.FirstName" required />

          <div class="hint" ng-if="showHints">Tell us what we should call you!</div>

          <div ng-messages="driverForm.name.$error" ng-if="!showHints">
         <%--   <div ng-message="required">Name is required.</div>
            <div ng-message="md-maxlength">The name has to be less than 30 characters long.</div>--%>
          </div>
        </md-input-container>

                            </div>
                            <div class="col-sm-3">
                                <md-input-container flex>
                                <label for="empLastNameNo">Last Name</label>
                                <input type="text" ng-model="employee.LastName" required />
                            </md-input-container>
                            </div>
                            <div class="col-sm-6">
                                <md-input-container flex>
                                        <label for="employeeType">Employee Type</label>
                                        <md-select id="employeeType" ng-model="employeeType" required>
                                            <!-- can check for null -->
                                            <div ng-if="employeePrefix.employeeTypes.length > 0 == false"><md-progress-circular md-mode="indeterminate" md-diameter="26"></md-progress-circular></div>
                                            <md-option ng-repeat="employeeType in employeePrefix.employeeTypes" ng-value="employeeType">
                                                {{employeeType.Name}}
                                            </md-option>
                                        </md-select>
                                    </md-input-container>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3">
                                <md-input-container flex>
                                <label for="empStrtAddress">Street Adress</label>
                                <input type="text" ng-model="employee.StreetAdd" required />
                            </md-input-container>
                            </div>
                            <div class="col-sm-3">
                                <md-input-container flex>
                                <label for="requestNo">City</label>
                                <input type="text"  ng-model="employee.City" required />
                            </md-input-container>
                            </div>
                            <div class="col-sm-3">
                                <md-input-container flex>
                                <label for="requestNo">State</label>
                                <input type="text" ng-model="employee.State" required />
                            </md-input-container>
                            </div>
                            <div class="col-sm-3">
                                <md-input-container flex>
                                <label for="requestNo">Zip Code</label>
                                <input type="text"  ng-model="employee.ZipCode" required />
                            </md-input-container>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-6">
                                <%--  <md-input-container flex>
                                <label for="Ph">Phone No</label>
                                <input ng-model="employee.Ph" type="number"  required />
                            </md-input-container>--%>

                                <md-input-container class="md-block" flex-gt-sm>
          <label>Phone Number</label>
          <input name="phone" ng-model="employee.Ph" ng-pattern="/^[(][0-9]{3}[)] [0-9]{3}-[0-9]{4}$/"  required />

          <div class="hint" ng-show="showHints">(###) ###-####</div>

          <div ng-messages="driverForm.phone.$error" ng-hide="showHints">
            <div ng-message="pattern">(###) ###-#### - Please enter a valid phone number.</div>
          </div>
        </md-input-container>

                            </div>
                            <div class="col-sm-6">
                                <md-input-container flex>
                                <label for="requesterName">Password</label>
                                <input ng-model="employee.DrvPass" type="text" required />
                            </md-input-container>
                            </div>
                        </div>

                        <div layout-gt-sm="row">
                            <div class="col-sm-6">
                                <md-input-container flex>
                                             <label for="Email">Email</label>
                                            <input ng-model="employee.Email" type="email" required />
                                    </md-input-container>
                            </div>

                            <div class="col-sm-6">
                                <md-input-container flex>
                                        <label for="storeInfo">Select Store</label>
                                        <md-select id="storeInfo" ng-model="storeInfo" required>
                                            <!-- can check for null -->
                                            <div ng-if="employeePrefix.storeInfos.length > 0 == false"><md-progress-circular md-mode="indeterminate" md-diameter="26"></md-progress-circular></div>
                                            <md-option ng-repeat="storeInfo in employeePrefix.storeInfos" ng-value="storeInfo">
                                                {{storeInfo.StoreName}}
                                            </md-option>
                                        </md-select>
                                    </md-input-container>

                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <md-input-container class="md-block" flex-gt-xs>
                                    <md-checkbox ng-model="employee.Active" aria-label="" class="md-warn md-align-top-left" flex>Active</md-checkbox>
                                </md-input-container>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <label></label>
                                <div class="crib-up-box income">
                                    <md-button value="Save" class="md-raised md-primary mg-btn-pv-sub-tbl" ng-show="update" ng-click="SaveEmployee()">Submit</md-button>
                                    <md-button value="Update" class="md-raised md-primary mg-btn-pv-sub-tbl" ng-hide="update" ng-click="UpdateEmployee()">Update</md-button>
                                    <md-button value="Cancel" class="md-raised md-warn mg-btn-pv-sub-tbl" ng-click="CancelEmployee()">Cancel</md-button>

                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <table class="table table-bordered table-hover">
                                    <thead>
                                        <tr>
                                            <th>Name</th>
                                            <th>Street Address</th>
                                            <th>City</th>
                                            <th>State</th>
                                            <th>ZipCode</th>
                                            <th>Phone #</th>
                                            <th>Email</th>
                                            <th>Employee Type</th>
                                            <th>Active</th>
                                            <th>Store</th>
                                            <th>
                                                <span class="fa fa-cog"></span>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr ng-repeat="employee in employees">
                                            <td>{{employee.FirstName}} 
                                             {{employee.LastName}} 
                                            </td>
                                            <td>{{employee.StreetAdd}} 
                                            </td>
                                            <td>{{employee.City}} 
                                            </td>
                                            <td>{{employee.State}}</td>
                                            <td>{{employee.ZipCode}}</td>
                                            <td>{{employee.Ph}} 
                                            </td>
                                            <td>{{employee.Email}} 
                                            </td>
                                            <td>
                                                <div ng-repeat="oemployeeType in employeePrefix.employeeTypes | filter:{Id:employee.EmployeeTypeId}:true">
                                                    {{oemployeeType.Name}}
                                                </div>
                                            </td>
                                            <td>
                                                <md-checkbox ng-model="employee.Active" ng-disabled="true" aria-label="" class="md-warn md-align-top-left" flex></md-checkbox>
                                            </td>
                                            <td>
                                                <%--  {{employee.StoreID}} --%>
                                                <div ng-repeat="oStore in employeePrefix.storeInfos | filter:{StoreID:employee.StoreID}:true">
                                                    {{oStore.StoreName}}
                                                </div>
                                            </td>
                                            <td>
                                                <a ng-click="DeleteEmployee(employee)">
                                                    <span class="glyphicon glyphicon-remove"></span>
                                                </a>
                                                <a ng-click="EditEmployee(employee)">
                                                    <span class="glyphicon glyphicon-edit"></span>
                                                </a>
                                            </td>

                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%: Scripts.Render("~/bundles/AngularJs") %>
    <script src="../App/controller/driverCtrl.js"></script>
</asp:Content>
