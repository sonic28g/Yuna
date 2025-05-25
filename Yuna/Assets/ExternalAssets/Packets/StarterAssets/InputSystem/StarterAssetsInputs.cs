using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool aim;
		public bool shoot;
		public bool interact;
		public bool showMenu;
		public bool crouch;
		public bool triggerJournal;

		public Action DialogueSkip;
        public Action DialogueNext;

        [Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		[Header("Pause")]
		[SerializeField] private string _playerActionMap = "Player";
		[SerializeField] private string _pauseActionMap = "UI";
		private PlayerInput _playerInput;
		private readonly List<object> _stoppers = new();
		

		private void Awake() => _playerInput = GetComponent<PlayerInput>();

		public void ResumeInput(object stopper)
		{
			// Remove the stopper from the list + Resume the input if there is no stoppers
			_stoppers.Remove(stopper);
			if (_stoppers.Count == 0) _playerInput.SwitchCurrentActionMap(_playerActionMap);
		}

		public void PauseInput(object stopper)
		{
			// Add the stopper to the list + Pause the input
			_stoppers.Add(stopper);
			_playerInput.SwitchCurrentActionMap(_pauseActionMap);
		}


		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnAim(InputValue value)
		{
			AimInput(value.isPressed);
		}

		public void OnShoot(InputValue value)
		{
			ShootInput(value.isPressed);
		}

		public void OnCrouch(InputValue value)
		{
			CrouchInput(value.isPressed);
		}

		public void OnInteract(InputValue value)
		{
			InteractInput(value.isPressed);
		}

		public void OnShowMenu(InputValue value)
		{
			ShowMenuInput(value.isPressed);
		}

		public void OnTriggerJournal(InputValue value)
		{
			TriggerJournalInput(value.isPressed);
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		public void OnDialogueSkip(InputValue _) => DialogueSkip?.Invoke();
        public void OnDialogueNext(InputValue _) => DialogueNext?.Invoke();


        public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void AimInput(bool newAimState)
		{
			aim = newAimState;
		}

		public void ShootInput(bool newShootState)
		{
			shoot = newShootState;
		}

		public void CrouchInput(bool newCrouchState)
		{
			crouch = newCrouchState;
		}

		public void InteractInput(bool newInteractState)
		{
			interact = newInteractState;
		}

		private void ShowMenuInput(bool newShowMenuState)
		{
			showMenu = newShowMenuState;
		}

		private void TriggerJournalInput(bool newTriggerJournalState)
		{
			triggerJournal = newTriggerJournalState;
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	

	}
	
}