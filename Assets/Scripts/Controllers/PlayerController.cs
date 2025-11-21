using Events;
using System.Linq;
using UnityEngine;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        public Models.Hand Hand { get; private set; }
        private Models.Expression.Expression expression;

        public int Credits { get; set; }

        private bool isSubmitAvailable;

        private bool isSquareRootPending;
        private Models.Cards.SpecialCard pendingSquareRootCard;

        private void Awake()
        {
            Initialize();

            ActorManager.SetPlayer(this);
        }

        private void Initialize()
        {
            Hand = new Models.Hand();
            expression = new Models.Expression.Expression();

            isSubmitAvailable = false;

            isSquareRootPending = false;
            pendingSquareRootCard = null;
        }

        void OnEnable()
        {
            Events.GameEvents.OnRoundStarted += HandleRoundStarted;
            Events.GameEvents.OnResetClicked += HandleResetClicked;
            Events.CardEvents.OnCardClicked += HandleCardClicked;
            Events.GameEvents.OnSubmitAvailabilityChanged += HandleSubmitAvailabilityChanged;
        }

        void OnDisable()
        {
            Events.GameEvents.OnRoundStarted -= HandleRoundStarted;
            Events.GameEvents.OnResetClicked -= HandleResetClicked;
            Events.CardEvents.OnCardClicked -= HandleCardClicked;
            Events.GameEvents.OnSubmitAvailabilityChanged -= HandleSubmitAvailabilityChanged;
        }

        public void ResetHand()
        {
            Hand = new Models.Hand();
        }

        public void Prepare()
        {
            expression.Clear();
            Hand.ResetCardsUsage();
            
            isSubmitAvailable = false;
            isSquareRootPending = false;
            pendingSquareRootCard = null;
        }

        public void AddCard(Models.Cards.Card card)
        {
            if (Hand == null)
            {
                Debug.LogWarning("[PlayerController] 손패가 설정되지 않았습니다.");
                return;
            }

            Hand.AddCard(card);
        }

        public Models.Expression.Expression GetExpression()
        {
            return expression.Clone();
        }
        private void HandleRoundStarted()
        {
            ResetHand();
            Prepare();
            Events.UIEvents.InvokeExpressionUpdated("");
        }

        private void HandleResetClicked()
        {
            Prepare();
            Events.UIEvents.InvokeExpressionUpdated("");
        }

        private void HandleSubmitAvailabilityChanged(bool canSubmit)
        {
            isSubmitAvailable = canSubmit;

            ShowCompletionStatus();
        }

        private void HandleCardClicked(Models.Cards.Card card)
        {
            if (card is Models.Cards.NumberCard numberCard)
            {
                HandleNumberCardClicked(numberCard);
            }
            else if (card is Models.Cards.OperatorCard operatorCard)
            {
                HandleOperatorCardClicked(operatorCard);
            }
            else if (card is Models.Cards.SpecialCard specialCard)
            {
                HandleSpecialCardClicked(specialCard);
            }
        }

        private void HandleNumberCardClicked(Models.Cards.NumberCard numberCard)
        {
            if (!expression.ExpectingNumber())
            {
                Events.UIEvents.InvokeStatusTextUpdated("지금은 연산자 카드를 눌러주세요");
                return;
            }

            if (!Hand.NumberCards.Contains(numberCard))
            {
                Debug.LogWarning("[PlayerController] 손패에 없는 숫자 카드입니다.");
                return;
            }

            bool applySquareRoot = isSquareRootPending && pendingSquareRootCard != null;

            expression.AddNumber(numberCard.Value, applySquareRoot);
            numberCard.MarkAsUsed();
            Events.CardEvents.InvokeCardConsumed(numberCard);

            if (applySquareRoot)
            {
                pendingSquareRootCard.MarkAsUsed();
                Events.CardEvents.InvokeCardConsumed(pendingSquareRootCard);

                isSquareRootPending = false;
                pendingSquareRootCard = null;

                Events.UIEvents.InvokeStatusTextUpdated("√ 카드가 적용되었습니다\n 연산자를 선택하세요");
            }

            Events.UIEvents.InvokeExpressionUpdated(expression.ToString());

            bool hasUnusedNumbers = HasUnusedNumberCards();

            if (!hasUnusedNumbers && !expression.ExpectingNumber())
            {
                ShowCompletionStatus();
            }
            else if (expression.ExpectingNumber())
            {
                Events.UIEvents.InvokeStatusTextUpdated("숫자 카드를 눌러주세요");
            }
            else
            {
                Events.UIEvents.InvokeStatusTextUpdated("연산자 카드를 눌러주세요");
            }
        }


        private void HandleOperatorCardClicked(Models.Cards.OperatorCard operatorCard)
        {
            if (expression.ExpectingNumber() || expression.IsEmpty())
            {
                Events.UIEvents.InvokeStatusTextUpdated("지금은 숫자 카드를 눌러주세요");
                return;
            }

            if (!HasUnusedNumberCards())
            {
                Events.UIEvents.InvokeStatusTextUpdated("남은 숫자가 없어 연산자를 더 선택할 수 없습니다");
                return;
            }

            if (!Hand.OperatorCards.Contains(operatorCard))
            {
                return;
            }

            expression.AddOperator(operatorCard.Operator);
            operatorCard.MarkAsUsed();
            Events.CardEvents.InvokeCardConsumed(operatorCard);

            Events.UIEvents.InvokeExpressionUpdated(expression.ToString());

            if (expression.ExpectingNumber())
            {
                Events.UIEvents.InvokeStatusTextUpdated("숫자 카드를 눌러주세요");
            }
        }

        private void HandleSpecialCardClicked(Models.Cards.SpecialCard specialCard)
        {
            if (!Hand.SpecialCards.Contains(specialCard))
            {
                return;
            }

            if (specialCard.Type == Algorithm.Operator.OperatorType.Multiply)
            {
                HandleMultiplyCardClicked(specialCard);
                return;
            }

            if (specialCard.Type == Algorithm.Operator.OperatorType.SquareRoot)
            {
                HandleSquareRootCardClicked(specialCard);
                return;
            }
        }

        private void HandleMultiplyCardClicked(Models.Cards.SpecialCard multiplyCard)
        {
            if (expression.IsEmpty() || expression.ExpectingNumber())
            {
                Events.UIEvents.InvokeStatusTextUpdated("숫자를 배치한 후에 × 카드를 눌러주세요");
                return;
            }

            if (!HasUnusedNumberCards())
            {
                Events.UIEvents.InvokeStatusTextUpdated("남은 숫자가 없어 × 카드를 사용할 수 없습니다");
                return;
            }

            expression.AddOperator(new Algorithm.Operator(Algorithm.Operator.OperatorType.Multiply));
            multiplyCard.MarkAsUsed();
            Events.CardEvents.InvokeCardConsumed(multiplyCard);

            Events.UIEvents.InvokeExpressionUpdated(expression.ToString());
            Events.UIEvents.InvokeStatusTextUpdated("숫자 카드를 눌러주세요");
        }

        private void HandleSquareRootCardClicked(Models.Cards.SpecialCard squareRootCard)
        {
            if (isSquareRootPending)
            {
                Events.UIEvents.InvokeStatusTextUpdated("이미 준비된 √ 카드가 있습니다\n 숫자를 선택하세요");
                return;
            }

            if (!expression.ExpectingNumber())
            {
                Events.UIEvents.InvokeStatusTextUpdated("연산자를 선택한 후 숫자를 넣을 차례에 √ 카드를 눌러주세요");
                return;
            }

            bool hasAvailableNumber = Hand.NumberCards.Any(numberCard =>
            {
                numberCard.MarkAsUnused();

                return !numberCard.IsUsed;
            });

            if (!hasAvailableNumber)
            {
                Events.UIEvents.InvokeStatusTextUpdated("남은 숫자 카드가 없어 √ 카드를 사용할 수 없습니다");
                return;
            }

            isSquareRootPending = true;
            pendingSquareRootCard = squareRootCard;

            Events.UIEvents.InvokeStatusTextUpdated("다음에 선택하는 숫자에 √가 적용됩니다\n 숫자를 골라주세요");
        }

        private bool HasUnusedNumberCards()
        {
            if (Hand == null)
            {
                return false;
            }

            foreach (var card in Hand.NumberCards)
            {
                if (!card.IsUsed)   
                {
                    return true;
                }
            }

            return false;
        }

        public bool NeedsSpecialCardUsageReminder()
        {
            if (Hand == null || expression == null)
            {
                return false;
            }

            if (expression.IsEmpty())
            {
                return false;
            }

            bool expressionComplete = !HasUnusedNumberCards() && !expression.ExpectingNumber();

            if (!expressionComplete)
            {
                return false;
            }

            return !HasUsedRequiredSpecialCards();
        }

        private void ShowCompletionStatus()
        {
            if (HasUnusedNumberCards() || expression == null || expression.ExpectingNumber())
            {
                return;
            }

            if (!HasUsedRequiredSpecialCards())
            {
                Events.UIEvents.InvokeStatusTextUpdated("제출하려면 받은 √와 × 카드를 모두 사용해야 합니다");
                return;
            }

            if (isSubmitAvailable)
            {
                Events.UIEvents.InvokeStatusTextUpdated("수식을 완성했습니다\n제출 버튼으로 확인해 보세요");
            }
            else
            {
                Events.UIEvents.InvokeStatusTextUpdated("10초 후 제출 버튼이 활성화됩니다");
            }
        }

        public bool HasUsedRequiredSpecialCards()
        {
            if (Hand == null || expression == null)
            {
                return false;
            }

            if (expression.IsEmpty())
            {
                return false;
            }

            foreach (var specialCard in Hand.SpecialCards)
            {
                if ((specialCard.Type == Algorithm.Operator.OperatorType.Multiply ||
                     specialCard.Type == Algorithm.Operator.OperatorType.SquareRoot) &&
                    !specialCard.IsUsed)
                {
                    return false;
                }
            }

            return true;
        }

    }
}
