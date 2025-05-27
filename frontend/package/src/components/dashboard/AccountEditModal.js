import React from 'react';
import {
  Modal,
  ModalHeader,
  ModalBody,
  ModalFooter,
  Form,
  FormGroup,
  Label,
  Input,
  Button
} from 'reactstrap';
import { FilePond } from 'react-filepond';
import 'filepond/dist/filepond.min.css';

const AccountEditModal = ({
  isModalOpen,
  setIsModalOpen,
  selectedCompte,
  setSelectedCompte,
  handleSave,
  files,
  setFiles,
  serverOptions,
  styles
}) => {
  const handleInputChange = (field, value) => {
    setSelectedCompte((prev) => ({
      ...prev,
      [field]: value
    }));
  };

  return (
    <Modal isOpen={isModalOpen} toggle={() => setIsModalOpen(false)}>
      <ModalHeader toggle={() => setIsModalOpen(false)}>
        Modifier ce compte
      </ModalHeader>
      <ModalBody>
        <Form>
          <div style={styles.formContainer}>
          <FormGroup style={styles.formGroup}>
  <Label for="username">Username</Label>
  <Input
    id="username"
    value={selectedCompte.userName || ''}
    onChange={(e) =>
      setSelectedCompte((prev) => ({
        ...prev,
        userName: e.target.value,
      }))
    }
    placeholder="Enter your username"
    type="text"
  />
</FormGroup>

<FormGroup style={styles.formGroup}>
  <Label for="email">Email</Label>
  <Input
    id="email"
    value={selectedCompte.email || ''}
    onChange={(e) =>
      setSelectedCompte((prev) => ({
        ...prev,
        email: e.target.value,
      }))
    }
    placeholder="Enter your email"
    type="email"
  />
</FormGroup>

          </div>

          {/* <div style={styles.formContainer}>
            <FormGroup style={styles.formGroup}>
              <Label for="password">Old Password</Label>
              <Input
                id="password"
                placeholder="Enter your old password"
                type="password"
              />
            </FormGroup>
            <FormGroup style={styles.formGroup}>
              <Label for="verifyPassword">New Password</Label>
              <Input
                id="verifyPassword"
                placeholder="Enter your new password"
                type="password"
              />
            </FormGroup>
          </div> */}

          <FormGroup>
            <Label for="role">Role</Label>
            <Input
              id="role"
              name="role"
              type="select"
              value={selectedCompte.role || ''}
              onChange={(e) => setSelectedCompte({ ...selectedCompte, role: e.target.value })}
            >
              <option value="">Select a role</option>
              <option value="Medecin">Medecin</option>
              <option value="Patient">Patient</option>
              <option value="Pharmacien">Pharmacien</option>
              <option value="Admin">Admin</option>
            </Input>
          </FormGroup>

          <FormGroup>
            <Label>Photo d'identité</Label>
            <div style={{ width: '80%', margin: 'auto', padding: '1%' }}>
              <FilePond
                files={files}
                acceptedFileTypes="image/*"
                onupdatefiles={setFiles}
                allowMultiple={true}
                server={serverOptions()}
                name="file"
              />
            </div>
          </FormGroup>
        </Form>
      </ModalBody>
      <ModalFooter>
        <Button color="primary" onClick={handleSave}>
          Enregistrer
        </Button>
        <Button color="secondary" onClick={() => setIsModalOpen(false)}>
          Annuler
        </Button>
      </ModalFooter>
    </Modal>
  );
};

export default AccountEditModal;
