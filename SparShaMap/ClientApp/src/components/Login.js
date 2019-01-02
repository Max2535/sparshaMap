import React from 'react';
import { Button, Form, FormGroup, Label, Input, Alert, Container, Row, Col } from 'reactstrap';
import SweetAlert from 'react-bootstrap-sweetalert';

import { FaCalendar } from 'react-icons/fa';

export default class Login extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            username: "",
            password: "",
            alert: null,
        };
        this.handleChange = this.handleChange.bind(this);
    }
    handleChange(event) {
        if (event.target.id === "username") {
            this.setState({ username: event.target.value });

        } else if (event.target.id === "password") {
            this.setState({ password: event.target.value });

        }
    }
    Login = async () => {
        console.log(this.state.username);
        console.log(this.state.password);
        try {
            const url = `api/auth/login`;
            const response = await fetch(url, {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(
                    {
                        username: this.state.username,
                        password: this.state.password
                    })
            });
            const json = await response.json();
            if (json.code === '500') {
                const getAlert = () => (
                    <SweetAlert
                        warning
                        confirmBtnText="ตกลง"
                        confirmBtnBsStyle="danger"
                        cancelBtnBsStyle="default"
                        title={json.detail}
                        onConfirm={() => this.setState({ alert:null })}
                    >กรุณาลองอีกครั้ง
                </SweetAlert>
                );

                this.setState({
                    alert: getAlert()
                });
            } else {
                sessionStorage.setItem('userData', json);
                window.location.href = "/";//goto index
            }
        } catch (error) {
            alert(error);
        }
    }
    render() {
        return (
            <Container style={{ marginTop: 10 }}>
                {this.state.alert}
                <Row>
                    <Col sm="12" md={{ size: 6, offset: 3 }}>
                        <Alert color="warning"><h3>กรุณาเข้าสู่ระบบ</h3></Alert>
                        <Form>
                            <FormGroup>
                                <Label for="user">ชื่อผู้ใช้</Label>
                                <Input type="text" id="username" placeholder="กรุณาระบบ ชื่อผู้ใช้" onChange={this.handleChange} />
                            </FormGroup>
                            <FormGroup>
                                <Label for="password">รหัสผ่าน</Label>
                                <Input type="password" id="password" placeholder="กรุณาระบุ รหัสผ่าน" onChange={this.handleChange}/>
                            </FormGroup>
                            <FormGroup check>
                                <Label check><Input type="checkbox" />{' '}จำชื่อผู้ใช้และรหัสผ่าน</Label>
                            </FormGroup>
                        </Form>
                        <Col sm="12" md={{ size: 6, offset: 3 }}>
                            <center>
                                <Button style={{ margin: 5 }} color="success" onClick={() => this.Login()}>เข้าสู่ระบบ</Button>
                                <Button style={{ margin: 5 }} color="danger">ยกเลิก</Button>
                            </center>
                    </Col>
                    </Col>
                </Row>
            </Container>

        );
    }
}