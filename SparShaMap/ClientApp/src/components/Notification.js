import React, { Component } from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { actionCreators } from '../store/users';
import { Col, FormText, Button, Modal, ModalHeader, ModalBody, ModalFooter, Input, Label, Form, FormGroup } from 'reactstrap';
import SweetAlert from 'react-bootstrap-sweetalert';
import ReactLoading from 'react-loading';
class Notification extends Component {
    constructor(props) {
        super(props);

        this.state = {
            alert: null,
            modal: false
        };
        this.toggle = this.toggle.bind(this);
    }

    toggle() {
        this.setState({
            modal: !this.state.modal
        });
    }

    componentWillMount() {
        // This method runs when the component is first added to the page
        const startDateIndex = parseInt(this.props.match.params.startDateIndex, 10) || 0;
        this.props.requestWeatherForecasts(startDateIndex);
    }

    componentWillReceiveProps(nextProps) {
        // This method runs when incoming props (e.g., route params) change
        const startDateIndex = parseInt(nextProps.match.params.startDateIndex, 10) || 0;
        this.props.requestWeatherForecasts(startDateIndex);
    }
    deleteData(name) {
        const getAlert = () => (
            <SweetAlert
                warning
                showCancel
                confirmBtnText="ใช่"
                cancelBtnText="ไม่"
                confirmBtnBsStyle="danger"
                cancelBtnBsStyle="default"
                title={<h4><span>คุณต้องการลบ สมาชิก <b>{name}</b>!หรือไม่</span></h4>}
                onConfirm={() => this.confirmAlert()}
                onCancel={() => this.hideAlert()}
            >You will not be able to recover this imaginary file
                </SweetAlert>
        );

        this.setState({
            alert: getAlert()
        });
    }

    loadData() {
        const getAlert = () => (
            <SweetAlert title="กรุณารอสักครู่">
                <span>A custom <span style={{ color: '#F8BB86' }}>html</span> message.</span>
            </SweetAlert>
        );

        this.setState({
            alert: getAlert()
        });
    }

    hideAlert() {
        console.log('Hiding alert...');
        this.setState({
            alert: null
        });
    }
    confirmAlert() {

        const getAlert = () => (
            <SweetAlert success title="ลบข้อมูลสำเร็จ!" confirmBtnText="ตกลง" onConfirm={() => this.hideAlert()}>You clicked the button!</SweetAlert>
        );

        this.setState({
            alert: getAlert()
        });
    }
    render() {
        const prevStartDateIndex = (this.props.startDateIndex || 0) - 5;
        const nextStartDateIndex = (this.props.startDateIndex || 0) + 5;

        console.log("isLoading:" + this.props.isLoading);
        console.log("nextStartDateIndex:" + nextStartDateIndex);
        return (
            <div>
                <h1>จัดการสมาชิก</h1>
                {this.props.isLoading ?
                    <center><ReactLoading type={"spokes"} color="#000" /></center> :
                    <div>
                        <table className='table'>
                            <thead>
                                <tr>
                                    <th>ลำดับ</th>
                                    <th>ชื่อ</th>
                                    <th>สกุล</th>
                                    <th>รหัสสมาชิก</th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody>
                                {this.props.jsonData.map((data, index) =>
                                    <tr key={this.props.startDateIndex + index + 1}>
                                        <td>{this.props.startDateIndex + index + 1}</td>
                                        <td>{data.FIRST_NAME_TH}</td>
                                        <td>{data.LAST_NAME_TH}</td>
                                        <td>{data.CUS_ID}{' '}</td>
                                        <td><Button onClick={()=>alert('ส่งเรียบร้อย')} color="success">ส่ง</Button>{' '}<Button onClick={this.toggle} color="warning">แก้ไข</Button>{' '}
                                            <a onClick={() => this.deleteData(data.FIRST_NAME_TH)} className='btn btn-danger'>
                                                <i className="fa fa-trash" aria-hidden="true"></i> ลบ
                                        </a>
                                            {this.state.alert}{' '}</td>
                                    </tr>
                                )}
                            </tbody>
                        </table>
                        <p className='clearfix'>
                            <b className='text-left'>
                                หน้าที่ {"page"} จาก {"page"}
                            </b>
                            <div className="text-right">
                                {prevStartDateIndex > 0 ? <Link className='btn btn-default pull-left' to={`/Notification/${prevStartDateIndex}`}><Button color="primary">ก่อนหน้า</Button></Link> : ""}
                                {/*ต้อง Check จาก total page*/}
                                <Link className='btn btn-default pull-right' to={`/Notification/${nextStartDateIndex}`}><Button color="primary">ถัดไป</Button></Link>
                            </div>
                        </p>
                    </div>
                }
                <Modal isOpen={this.state.modal} toggle={this.toggle} className="modal-dialog modal-lg" backdrop={'static'}>
                    <ModalHeader toggle={this.toggle}>Modal title</ModalHeader>
                    <ModalBody>
                        <Form>
                            <FormGroup row>
                                <Label for="exampleEmail" sm={2}>Email</Label>
                                <Col sm={10}>
                                    <Input type="email" name="email" id="exampleEmail" placeholder="with a placeholder" />
                                </Col>
                            </FormGroup>
                            <FormGroup row>
                                <Label for="examplePassword" sm={2}>Password</Label>
                                <Col sm={10}>
                                    <Input type="password" name="password" id="examplePassword" placeholder="password placeholder" />
                                </Col>
                            </FormGroup>
                            <FormGroup row>
                                <Label for="exampleSelect" sm={2}>Select</Label>
                                <Col sm={10}>
                                    <Input type="select" name="select" id="exampleSelect" />
                                </Col>
                            </FormGroup>
                            <FormGroup row>
                                <Label for="exampleText" sm={2}>Text Area</Label>
                                <Col sm={10}>
                                    <Input type="textarea" name="text" id="exampleText" />
                                </Col>
                            </FormGroup>
                            <FormGroup row>
                                <Label for="exampleFile" sm={2}>File</Label>
                                <Col sm={10}>
                                    <Input type="file" name="file" id="exampleFile" />
                                    <FormText color="muted">
                                        This is some placeholder block-level help text for the above input.
                                        It's a bit lighter and easily wraps to a new line.</FormText>
                                </Col>
                            </FormGroup>
                            <FormGroup tag="fieldset" row>
                                <legend className="col-form-label col-sm-2">Radio Buttons</legend>
                                <Col sm={10}>
                                    <FormGroup check>
                                        <Label check>
                                            <Input type="radio" name="radio2" />{' '}
                                            Option one is this and that—be sure to include why it's great</Label>
                                    </FormGroup>
                                    <FormGroup check>
                                        <Label check>
                                            <Input type="radio" name="radio2" />{' '}
                                            Option two can be something else and selecting it will deselect option one</Label>
                                    </FormGroup>
                                    <FormGroup check disabled>
                                        <Label check>
                                            <Input type="radio" name="radio2" disabled />{' '}
                                            Option three is disabled</Label>
                                    </FormGroup>
                                </Col>
                            </FormGroup>
                            <FormGroup row>
                                <Label for="checkbox2" sm={2}>Checkbox</Label>
                                <Col sm={{ size: 10 }}>
                                    <FormGroup check>
                                        <Label check>
                                            <Input type="checkbox" id="checkbox2" />{' '}
                                            Check me out</Label>
                                    </FormGroup>
                                </Col>
                            </FormGroup>
                        </Form>
                    </ModalBody>
                    <ModalFooter>
                        <Button color="primary" onClick={this.toggle}>Do Something</Button>{' '}
                        <Button color="secondary" onClick={this.toggle}>Cancel</Button>
                    </ModalFooter>
                </Modal>
            </div>
        );
    }
}

function renderPagination(props) {
    const prevStartDateIndex = (props.startDateIndex || 0) - 5;
    const nextStartDateIndex = (props.startDateIndex || 0) + 5;

    return <p className='clearfix text-center'>
        <Link className='btn btn-default pull-left' to={`/Notification/${prevStartDateIndex}`}>Previous</Link>
        <Link className='btn btn-default pull-right' to={`/Notification/${nextStartDateIndex}`}>Next</Link>
        {props.isLoading ? <span>Loading...</span> : []}
    </p>;
}

export default connect(
    state => state.users,
    dispatch => bindActionCreators(actionCreators, dispatch)
)(Notification);
